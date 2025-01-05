using System.Collections;
using UnityEngine;

public class SitCarState : PlayerOnFootState
{
    private MonoBehaviour _monoBehaviour;
    private bool _inAction = false;  
    private float _horizontalThreshold = 0.2f; // Порог для X и Z
    private float _verticalThreshold = 0.8f; // Порог для Y (например, 0.8f для допустимой разницы высоты)

    public SitCarState(StateMachine stateMachine, PlayerConfig config, MonoBehaviour monoBehaviour) : base(stateMachine, config)
    {
        _monoBehaviour = monoBehaviour;
    }

    public override void HandleInput()
    {
        if (!_inAction)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
                StateMachine.ChangeState<IdleState>();
        }
    }

    public override void LogicUpdate()
    {
        // Вычисляем расстояние до места посадки по осям X, Y и Z
        float distX = Mathf.Abs(Config.PlayerTransform.position.x - Config.VehicleEntryPosition.position.x);
        float distZ = Mathf.Abs(Config.PlayerTransform.position.z - Config.VehicleEntryPosition.position.z);
        float distY = Mathf.Abs(Config.PlayerTransform.position.y - Config.VehicleEntryPosition.position.y);

       //Debug.Log($"x:{distX} z:{distZ} y:{distY}");

        if (distX < _horizontalThreshold && distZ < _horizontalThreshold && distY < _verticalThreshold)
        {
            if (!_inAction && TryFindDriverSeat(out var seatTransform))
                _monoBehaviour.StartCoroutine(EnterCar(seatTransform));
        }
        else
        {
            Config.Animator.SetFloat("moveAmount", 1, 0.5f, Time.deltaTime);
            Velocity = Config.PlayerTransform.forward * Config.MoveSpeed;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!_inAction)
            RotatePlayerToCar();
    }

    private void RotatePlayerToCar()
    {
        Vector3 direction = Config.VehicleEntryPosition.position - Config.PlayerTransform.position;
        direction.y = 0; // Убираем вертикальную составляющую, чтобы избежать вращения по оси X и Z
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Config.PlayerTransform.rotation = Quaternion.RotateTowards(Config.PlayerTransform.rotation, targetRotation, Config.RotationSpeed);
    }

    private bool TryFindDriverSeat(out Transform transform)
    {
        transform = null;

        foreach (Seat seat in Config.VehicleSeats)
        {
            if (seat.SeatType == SeatType.DriversSeat)
            {
                transform = seat.Transform;
                return true;
            }
        }

        return false;
    }

    private IEnumerator EnterCar(Transform seatTransform)
    {
        _inAction = true;

        Config.CameraController.SetTarget(Config.PlayerTransform, Config.VehicleTransform, CameraType.Car);

        Config.DoorControl.OpenDoor();

        Config.CharacterController.enabled = false;

        Config.Animator.SetFloat("moveAmount", 0);

        Config.PlayerTransform.SetParent(seatTransform);

        Config.PlayerTransform.position = Config.VehicleEntryPosition.position;

        // Поворачиваем игрока в сторону места посадки
        Quaternion newRotation = Quaternion.Euler(Config.VehicleEntryPosition.rotation.eulerAngles.x,
            Config.VehicleEntryPosition.rotation.eulerAngles.y + 90, Config.VehicleEntryPosition.rotation.eulerAngles.z);
        Config.PlayerTransform.rotation = newRotation;
      
        Velocity = Vector3.zero;  
        Config.Animator.CrossFade("Enter Car", 0.2f);
        yield return null;
        var animState = Config.Animator.GetNextAnimatorStateInfo(1);
        yield return new WaitForSeconds(animState.length);
        Config.PlayerTransform.position = seatTransform.position;

        Config.DoorControl.CloseDoor();

        _inAction = false;
        StateMachine.ChangeState<DrivingState>();
    }
}

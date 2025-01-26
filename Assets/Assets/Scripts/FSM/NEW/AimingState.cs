using UnityEngine;

public abstract class AimingState : PlayerOnFootState
{
    public AimingState(StateMachine stateMachine, PlayerConfig config) : base(stateMachine, config)
    {
    }

    public override void HandleInput()
    {
        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");
        //Vector3 rotation = new Vector3(0, mouseX, 0);
        //_config.PlayerTransform.Rotate(rotation);


        base.HandleInput();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            StateMachine.ChangeState<IdleState>();
        }

        RotatePlayerTowardsCamera();
    }

    private void RotatePlayerTowardsCamera()
    {
        // ѕолучаем направление от игрока к камере
        Vector3 direction = Config.PlayerTransform.position - Config.CameraController.transform.position;
        direction.y = 0; // »грок не должен вращатьс€ по вертикали

        if (direction.magnitude > 0.1f) // ѕровер€ем, что направление достаточно велико
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Config.PlayerTransform.rotation = Quaternion.Slerp(Config.PlayerTransform.rotation, targetRotation, Config.RotationSpeed * Time.deltaTime);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && other.TryGetComponent(out IVehicle vehicle))
        {
            Config.VehicleTransform = other.transform;
            Config.VehicleEntryPosition = vehicle.VehicleEntryPosition;
            Config.VehicleSeats = vehicle.Seats;
            Config.DoorControl = vehicle;
            Config.Movable = vehicle;
            Config.Steerable = vehicle;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Config.VehicleTransform = null;
            Config.VehicleEntryPosition = null;
            Config.VehicleSeats = null;
            Config.DoorControl = null;
            Config.Movable = null;
            Config.Steerable = null;
        }
    }
}

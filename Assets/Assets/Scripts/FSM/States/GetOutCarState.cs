using System.Collections;
using UnityEngine;

public class GetOutCarState : State
{
    private PlayerConfig _config;
    private MonoBehaviour _monoBehaviour;
    private float _exitCarOffsetZ = -0.55f;
    private bool _inAction;

    public GetOutCarState(StateMachine stateMachine, PlayerConfig config, MonoBehaviour monoBehaviour) : base(stateMachine)
    {
        _config = config;
        _monoBehaviour = monoBehaviour;
    }

    public override void Enter()
    {
        _config.DoorControl.OpenDoor();
        _config.CameraController.SetTarget(_config.VehicleTransform, _config.PlayerTransform, CameraType.Character);
        _monoBehaviour.StartCoroutine(GetOutCar());
    }

    private IEnumerator GetOutCar()
    {
        _inAction = true;

        // Метод TransformPoint берет эти локальные координаты и преобразует их в мировые координаты с учетом текущей
        // ротации и позиции объекта. Это значит, что если наш транспорт повернут, TransformPoint правильно рассчитает,
        // куда именно должен переместиться персонаж в мировой системе координат.
        _config.PlayerTransform.position = _config.PlayerTransform.TransformPoint(new Vector3(0, 0, _exitCarOffsetZ));

        _config.Animator.CrossFade("Exit Car", 0.2f);
        yield return null;
        var animState = _config.Animator.GetNextAnimatorStateInfo(1);
        yield return new WaitForSeconds(animState.length);
        _config.PlayerTransform.SetParent(null);
        _config.CharacterController.enabled = true;
        _config.DoorControl.CloseDoor();
       _inAction = false;
        StateMachine.ChangeState<IdleState>();
    }
}

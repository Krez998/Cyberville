using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerConfig _playerOnFootConfig;
    private CameraController _cameraController;
    private StateMachine _stateMachine;


    [Header("Temp Settings")]
    [SerializeField] private bool _isShowStateInfo;
    [SerializeField] private string _currentState;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(
            _playerOnFootConfig.PlayerTransform.TransformPoint(_playerOnFootConfig.GroundCheckOffset),
            _playerOnFootConfig.GroundCheckRadius);
    }

    private void Awake()
    {
        _playerOnFootConfig.PlayerTransform = transform;
        _cameraController = Camera.main.GetComponent<CameraController>();
        _cameraController.SetTarget(transform, transform, CameraType.Character);
        _playerOnFootConfig.CameraController = _cameraController;
        _playerOnFootConfig.Animator = GetComponent<Animator>();
        _playerOnFootConfig.CharacterController = GetComponent<CharacterController>();

        _stateMachine = new StateMachine();
        _stateMachine.AddState(new IdleState(_stateMachine, _playerOnFootConfig));
        _stateMachine.AddState(new MoveState(_stateMachine, _playerOnFootConfig));
        _stateMachine.AddState(new MelleeFightingState(_stateMachine, _playerOnFootConfig, this));
        _stateMachine.AddState(new SitCarState(_stateMachine, _playerOnFootConfig, this));
        _stateMachine.AddState(new GetOutCarState(_stateMachine, _playerOnFootConfig, this));
        _stateMachine.AddState(new DrivingState(_stateMachine, _playerOnFootConfig));
        ////////////////////////////////////////////
        _stateMachine.AddState(new Aim_IdleState(_stateMachine, _playerOnFootConfig));
        _stateMachine.AddState(new Aim_WalkState(_stateMachine, _playerOnFootConfig));
        _stateMachine.AddState(new Aim_RunState(_stateMachine, _playerOnFootConfig));
        _stateMachine.AddState(new Aim_CrouchState(_stateMachine, _playerOnFootConfig));

        _stateMachine.ChangeState<IdleState>();
        //_stateMachine.ChangeState<Aim_IdleState>();
    }

    private void Update()
    {
        _stateMachine.CurrentState.HandleInput();
        _stateMachine.CurrentState.LogicUpdate();


        ///////////////////////////////////
        if (_isShowStateInfo)
            _currentState = _stateMachine.CurrentState.GetType().ToString();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();     
    }

    private void OnTriggerEnter(Collider other)
    {
        _stateMachine.CurrentState?.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _stateMachine.CurrentState?.OnTriggerExit(other);
    }
}

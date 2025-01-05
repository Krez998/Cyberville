using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerConfig _playerOnFootConfig;
    [SerializeField] private CurrentState _currentState;

    private CameraController _cameraController;  
    private StateMachine _stateMachine;


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
        _stateMachine.ChangeState<IdleState>();
    }

    private void Update()
    {
        _stateMachine.CurrentState.HandleInput();
        _stateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        _stateMachine.CurrentState.PhysicsUpdate();


        switch (_stateMachine.CurrentState)
        {
            case IdleState:
                _currentState = CurrentState.IdleState;
                break;
            case MoveState:
                _currentState = CurrentState.MoveState; 
                break;
            case MelleeFightingState:
                _currentState = CurrentState.FightingState;
                break;
            case SitCarState:
                _currentState = CurrentState.SitCarState;
                break;
            case GetOutCarState:
                _currentState = CurrentState.GetOutCarState;
                break;
            case DrivingState:
                _currentState = CurrentState.DrivingState;
                break;
        }
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

public enum CurrentState
{
    Default,
    IdleState,
    MoveState,
    FightingState,
    SitCarState,
    GetOutCarState,
    DrivingState
}

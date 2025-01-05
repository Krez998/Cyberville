using UnityEngine;

public class DrivingState : State
{
    private PlayerConfig _config;

    public DrivingState(StateMachine stateMachine, PlayerConfig config) : base(stateMachine)
    {
        _config = config;
    }

    public override void Enter()
    {
        _config.PlayerTransform.rotation = _config.VehicleTransform.rotation;        
        _config.Animator.Play("Driving", 1);
    }

    public override void HandleInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            _config.Movable.GoForward();
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            _config.Movable.Reverse();
        else if (Input.GetKey(KeyCode.Space))
            _config.Movable.Brake();
        else
            _config.Movable.Deceleration();


        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            _config.Steerable.TurnLeft(40f);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            _config.Steerable.TurnRight(40f);
        else
            _config.Steerable.StraightenSteeringWheel();


        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
        {
            StateMachine.ChangeState<GetOutCarState>();
        }
    }
}

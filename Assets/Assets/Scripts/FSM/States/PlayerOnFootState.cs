using System;
using UnityEngine;

public abstract class PlayerOnFootState : State
{
    protected PlayerConfig Config;
    protected float HorizontalInput;
    protected float VerticalInput;
    protected float MoveAmount;
    protected Vector3 Velocity;

    //protected float MoveAmount;
    //protected float InterpolatedVerticalInput;
    //protected float InterpolatedHorizontalInput;
    //private float _interpolationSpeed = 5f;

    public PlayerOnFootState(StateMachine stateMachine, PlayerConfig config) : base(stateMachine)
    {
        Config = config;
    }

    public override void HandleInput()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        VerticalInput = Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.Mouse0))
        {
            StateMachine.ChangeState<MelleeFightingState>();
        }

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return))
        {
            if (Config.VehicleEntryPosition != null)
                StateMachine.ChangeState<SitCarState>();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(MoveAmount > 0)
                StateMachine.ChangeState<Aim_WalkState>();

            StateMachine.ChangeState<Aim_IdleState>();
        }

    }


    //public override void LogicUpdate()
    //{
    //    Config.InterpolatedVerticalInput = (float)Math.Round(Mathf.Lerp(Config.InterpolatedVerticalInput, VerticalInput, _interpolationSpeed * Time.deltaTime), 3);
    //    Config.InterpolatedVerticalInput = Mathf.Abs(Config.InterpolatedVerticalInput) <= 0.005f ? 0 : Config.InterpolatedVerticalInput;

    //    Config.InterpolatedHorizontalInput = (float)Math.Round(Mathf.Lerp(Config.InterpolatedHorizontalInput, HorizontalInput, _interpolationSpeed * Time.deltaTime), 3);
    //    Config.InterpolatedHorizontalInput = Mathf.Abs(Config.InterpolatedHorizontalInput) <= 0.005f ? 0 : Config.InterpolatedHorizontalInput;

    //    CheckGround();
    //    Config.YSpeed += Config.IsGrounded ? 0 : Physics.gravity.y * Time.deltaTime;
    //    Velocity.y = Config.YSpeed;
    //}

    public override void LogicUpdate()
    {
        // test ////////////
        //Config.Animator.SetFloat(nameof(MoveAmount), MoveAmount); /// ****

        Config.YSpeed += Config.IsGrounded ? 0 : Physics.gravity.y * Time.deltaTime;
        Velocity.y = Config.YSpeed;
    }

    public override void PhysicsUpdate()
    {
        //Config.YSpeed += Config.IsGrounded ? 0 : Physics.gravity.y * Time.fixedDeltaTime;
        //Velocity.y = Config.YSpeed;

        CheckGround();

        if (Config.CharacterController.enabled)
            Config.CharacterController.Move(Velocity);
    }

    private void CheckGround()
    {
        Config.IsGrounded = Physics.CheckSphere(
            Config.PlayerTransform.TransformPoint(Config.GroundCheckOffset),
            Config.GroundCheckRadius, Config.GroundLayer);
    }
}

using System;
using UnityEngine;

public abstract class PlayerOnFootState : State
{
    protected PlayerConfig Config;
    protected float HorizontalInput;
    protected float VerticalInput;
    protected float MoveAmount;
    protected Vector3 Velocity;

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
    }

    public override void LogicUpdate()
    {
        Config.Animator.SetFloat("moveAmount", MoveAmount, 0.3f, Time.deltaTime);
    }

    public override void PhysicsUpdate()
    {
        CheckGround();

        Config.YSpeed += Config.IsGrounded ? 0 : Physics.gravity.y;
        Velocity.y = Config.YSpeed;

        if (Config.CharacterController.enabled)
            Config.CharacterController.Move(Velocity);
    }

    private void CheckGround()
    {
        Physics.CheckSphere(
            Config.PlayerTransform.TransformPoint(Config.GroundCheckOffset),
            Config.GroundCheckRadius, Config.GroundLayer);
    }
}

using System;
using UnityEngine;

public class Aim_WalkState : AimingState
{
    private Vector3 _moveInput;

    public Aim_WalkState(StateMachine stateMachine, PlayerConfig config) : base(stateMachine, config)
    {
    }

    public override void Enter()
    {
        Debug.Log(nameof(Aim_WalkState));

        Config.Animator.SetBool("isAiming", true);
        Config.Animator.SetBool("Walking", true);
        //Config.Animator.SetBool("Running", false);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            StateMachine.ChangeState<Aim_RunState>();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StateMachine.ChangeState<Aim_CrouchState>();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // test ////////////
        Config.Animator.SetFloat(nameof(MoveAmount), MoveAmount); /// ****


        MoveAmount = Mathf.Clamp01(Mathf.Abs(Config.Animator.GetFloat("hzinput"))
            + Mathf.Abs(Config.Animator.GetFloat("vinput")));
        //MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));

        Config.Animator.SetFloat("vinput", VerticalInput, 0.1f, Time.deltaTime);
        Config.Animator.SetFloat("hzinput", HorizontalInput, 0.1f, Time.deltaTime);
        //Config.Animator.SetFloat("vinput", VerticalInput);
        //Config.Animator.SetFloat("hzinput", HorizontalInput);

        var speed = VerticalInput < 0 ? Config.MoveSpeed * 0.6f : Config.MoveSpeed;

        _moveInput = new Vector3(HorizontalInput, 0, VerticalInput).normalized;
        Velocity = Config.PlayerTransform.TransformDirection(_moveInput) * speed * 0.4f * MoveAmount;

        if (Math.Round(MoveAmount, 1) == 0)
            StateMachine.ChangeState<Aim_IdleState>();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        Config.Animator.SetBool("isAiming", false);
        Config.Animator.SetBool("Walking", false);      
    }
}

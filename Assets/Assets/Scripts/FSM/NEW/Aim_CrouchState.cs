using UnityEngine;

public class Aim_CrouchState : AimingState
{
    private Vector3 _moveInput;

    public Aim_CrouchState(StateMachine stateMachine, PlayerConfig config) : base(stateMachine, config)
    {
    }

    public override void Enter()
    {
        Config.Animator.SetBool("isAiming", true);
        Config.Animator.SetBool("Crouching", true);    
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetKeyDown(KeyCode.C))
        {
            StateMachine.ChangeState<Aim_IdleState>();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            StateMachine.ChangeState<Aim_RunState>();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // test ////////////
        Config.Animator.SetFloat(nameof(MoveAmount), MoveAmount); /// ****

        MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));

        //Config.Animator.SetFloat("vinput", VerticalInput, 0.5f, Time.deltaTime);
        //Config.Animator.SetFloat("hzinput", HorizontalInput, 0.5f, Time.deltaTime);
        Config.Animator.SetFloat("vinput", VerticalInput);
        Config.Animator.SetFloat("hzinput", HorizontalInput);

        var speed = VerticalInput < 0 ? Config.MoveSpeed * 0.6f : Config.MoveSpeed;

        _moveInput = new Vector3(HorizontalInput, 0, VerticalInput).normalized;
        Velocity = Config.PlayerTransform.TransformDirection(_moveInput) * Config.MoveSpeed * 0.25f * MoveAmount;
    }

    public override void Exit()
    {
        Config.Animator.SetBool("Crouching", false);
    }
}

using UnityEngine;

public class Aim_RunState : AimingState
{
    private Vector3 _moveInput;

    public Aim_RunState(StateMachine stateMachine, PlayerConfig config) : base(stateMachine, config)
    {
    }

    public override void Enter()
    {
        Debug.Log(nameof(Aim_RunState));

        Config.Animator.SetBool("isAiming", true);
        Config.Animator.SetBool("Walking", true);
        Config.Animator.SetBool("Running", true);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (Input.GetKeyUp(KeyCode.LeftShift) || MoveAmount < 0.2f)
        {
            StateMachine.ChangeState<Aim_WalkState>();
        }
    }

    public override void LogicUpdate()
    {
        // test ////////////
        Config.Animator.SetFloat(nameof(MoveAmount), MoveAmount); /// ****

        base.LogicUpdate();
        MoveAmount = Mathf.Clamp01(Mathf.Abs(Config.Animator.GetFloat("hzinput"))
            + Mathf.Abs(Config.Animator.GetFloat("vinput")));
        //MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));

        Config.Animator.SetFloat("vinput", VerticalInput, 0.1f, Time.deltaTime);
        Config.Animator.SetFloat("hzinput", HorizontalInput, 0.1f, Time.deltaTime);
        //Config.Animator.SetFloat("vinput", VerticalInput);
        //Config.Animator.SetFloat("hzinput", HorizontalInput);

        var speed = VerticalInput < 0 ? Config.MoveSpeed * 0.6f : Config.MoveSpeed;

        _moveInput = new Vector3(HorizontalInput, 0, VerticalInput).normalized;
        Velocity = Config.PlayerTransform.TransformDirection(_moveInput) * speed * MoveAmount /*Config.Animator.GetFloat("moveAmount")*/;
    }

    public override void Exit()
    {
        //Config.Animator.SetBool("isAiming", false);
        Config.Animator.SetBool("Running", false);
    }
}

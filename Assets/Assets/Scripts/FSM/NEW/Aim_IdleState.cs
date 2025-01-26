using UnityEngine;

public class Aim_IdleState : AimingState
{
    public Aim_IdleState(StateMachine stateMachine, PlayerConfig config) : base(stateMachine, config)
    {
    }

    public override void Enter()
    {
        Debug.Log(nameof(Aim_IdleState));

        Config.Animator.SetBool("isAiming", true);
    }

    public override void HandleInput()
    {
        base.HandleInput();

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

        Config.Animator.SetFloat("vinput", VerticalInput, 0.1f, Time.deltaTime);
        Config.Animator.SetFloat("hzinput", HorizontalInput, 0.1f, Time.deltaTime);

        if (MoveAmount > 0.01f)
            StateMachine.ChangeState<Aim_WalkState>();
    }

    public override void Exit()
    {
        Config.Animator.SetBool("isAiming", false);
    }
}

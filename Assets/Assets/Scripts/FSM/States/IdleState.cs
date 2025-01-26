using UnityEngine;

public class IdleState : PlayerOnFootState
{
    public IdleState(StateMachine stateMachine, PlayerConfig config)
        : base(stateMachine, config)
    {
    }

    public override void Enter()
    {
        Debug.Log(nameof(IdleState));

        Config.Animator.SetBool("isAiming", false);
        Config.Animator.SetBool("Walking", false);

        base.Enter();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
        Config.Animator.SetFloat("moveAmount", MoveAmount, 0.3f, Time.deltaTime);
        
        if (MoveAmount > 0)
            StateMachine.ChangeState<MoveState>();
    }

    //public override void LogicUpdate()
    //{
    //    base.LogicUpdate();

    //    //Config.Animator.SetFloat("moveAmount", MoveAmount, 0.3f, Time.deltaTime); ***
    //    Config.Animator.SetFloat("vinput", Config.InterpolatedVerticalInput);
    //    Config.Animator.SetFloat("hzinput", Config.InterpolatedHorizontalInput);


    //    Config.MoveAmount = Mathf.Clamp01(Mathf.Abs(Config.InterpolatedHorizontalInput) + Mathf.Abs(Config.InterpolatedVerticalInput));
    //    if (Config.MoveAmount > 0f)
    //        StateMachine.ChangeState<MoveState>();
    //}

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car") && other.TryGetComponent(out IVehicle vehicle))
        {
            Config.VehicleTransform = other.transform;
            Config.VehicleEntryPosition = vehicle.VehicleEntryPosition;
            Config.VehicleSeats = vehicle.Seats;
            Config.DoorControl = vehicle;
            Config.Movable = vehicle;
            Config.Steerable = vehicle;
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Config.VehicleTransform = null;
            Config.VehicleEntryPosition = null;
            Config.VehicleSeats = null;
            Config.DoorControl = null;
            Config.Movable = null;
            Config.Steerable = null;
        }
    }
}

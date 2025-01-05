using UnityEngine;

public class IdleState : PlayerOnFootState
{
    public IdleState(StateMachine stateMachine, PlayerConfig config)
        : base(stateMachine, config)
    {
    }

    public override void Enter()
    {
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
        if (MoveAmount > 0)
            StateMachine.ChangeState<MoveState>();
    }

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

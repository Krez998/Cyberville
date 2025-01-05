using UnityEngine;

public class MoveState : PlayerOnFootState
{
    private Vector3 _moveInput;

    public MoveState(StateMachine stateMachine, PlayerConfig config)
        : base(stateMachine, config)
    {
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        MoveAmount = Mathf.Clamp01(Mathf.Abs(HorizontalInput) + Mathf.Abs(VerticalInput));
        _moveInput = new Vector3(HorizontalInput, 0, VerticalInput).normalized;
        Velocity = Config.PlayerTransform.forward * Config.MoveSpeed * Config.Animator.GetFloat("moveAmount");

        if (Config.Animator.GetFloat("moveAmount") < 0.1f && MoveAmount == 0)
        {
            StateMachine.ChangeState<IdleState>();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (MoveAmount > 0)
        {
            var targetRotation = Quaternion.LookRotation(Config.CameraController.PlanarRotation * _moveInput);

            Config.PlayerTransform.rotation = Quaternion.RotateTowards(Config.PlayerTransform.rotation, targetRotation,
                Config.RotationSpeed);
        }
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

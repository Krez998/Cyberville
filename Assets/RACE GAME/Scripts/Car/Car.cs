using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarEngine))]
[RequireComponent(typeof(GearBox))]
[RequireComponent(typeof(SteeringWheel))]
[RequireComponent(typeof(VehicleDoors))]
public class Car : MonoBehaviour, IVehicle
{
    public Transform CameraTarget => _cameraTarget;
    public Transform VehicleEntryPosition => _vehicleEntryPosition;
    public List<Seat> Seats => _seats;

    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _vehicleEntryPosition;
    [SerializeField] private List<Seat> _seats = new List<Seat>();


    [SerializeField] private float _mass;

    [Header("Engine Settings")]
    [SerializeField] private WheelDriveMode _wheelDriveMode;
    [SerializeField] private float _motorTorque;
    [SerializeField] private float _brakeTorque;

    [Header("Gearbox Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private int _numberOfGears;

    [Header("Steering Wheel Settings")]
    [SerializeField, Range(0, 40)] private float _rotationAngle;

    [Header("Audio Data")]
    [SerializeField] private AudioClip _acceleration;
    [SerializeField] private AudioClip _deceleration;
    [SerializeField] private AudioClip _idle;

    private Rigidbody _rigidbody;
    private CarEngine _carEngine;
    private GearBox _gearBox;
    private SteeringWheel _steeringWheel;
    private VehicleDoors _vehicleDoors;
    private CarEngineSounds _engineSounds;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carEngine = GetComponent<CarEngine>();
        _gearBox = GetComponent<GearBox>();
        _steeringWheel = GetComponent<SteeringWheel>();
        _vehicleDoors = GetComponent<VehicleDoors>();
        _engineSounds = GetComponent<CarEngineSounds>();

        _rigidbody.mass = _mass;

        _carEngine.SetData(_motorTorque, _brakeTorque, _wheelDriveMode);
        _gearBox.SetData(_speed, _numberOfGears);
        _engineSounds.SetData(_acceleration, _deceleration, _idle);
    }

    public void GoForward() => _carEngine.GoForward();

    public void Reverse() => _carEngine.Reverse();

    public void Brake() => _carEngine.Brake();

    public void Deceleration() => _carEngine.Deceleration();

    public void TurnLeft(float angle) => _steeringWheel.TurnLeft(_rotationAngle);

    public void TurnRight(float angle) => _steeringWheel.TurnRight(_rotationAngle);

    public void StraightenSteeringWheel() => _steeringWheel.StraightenSteeringWheel();

    public void OpenDoor() => _vehicleDoors.OpenDoor();

    public void CloseDoor() => _vehicleDoors.CloseDoor();
}
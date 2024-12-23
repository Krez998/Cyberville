using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CarEngine))]
[RequireComponent(typeof(GearBox))]
public class Car : MonoBehaviour, IVehicleInfoProvider
{
    public Transform CameraTarget => _cameraTarget;
    public Transform VehicleEntryPosition => _vehicleEntryPosition;

    public IInput input => throw new System.NotImplementedException();

    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Transform _vehicleEntryPosition;
    [SerializeField] private float _mass;

    [Header("Engine Settings")]
    [SerializeField] private WheelDriveMode _wheelDriveMode;
    [SerializeField] private float _motorTorque;
    [SerializeField] private float _brakeTorque;

    [Header("Gearbox Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private int _numberOfGears;

    [Header("Audio Data")]
    [SerializeField] private AudioClip _acceleration;
    [SerializeField] private AudioClip _deceleration;
    [SerializeField] private AudioClip _idle;

    private Rigidbody _rigidbody;
    private CarEngine _carEngine;
    private GearBox _gearBox;
    private CarEngineSounds _engineSounds;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carEngine = GetComponent<CarEngine>();
        _gearBox = GetComponent<GearBox>();
        _engineSounds = GetComponent<CarEngineSounds>();

        _rigidbody.mass = _mass;

        _carEngine.SetData(_motorTorque, _brakeTorque, _wheelDriveMode);
        _gearBox.SetData(_speed, _numberOfGears);
        _engineSounds.SetData(_acceleration, _deceleration, _idle);
    }
}
using UnityEngine;

[RequireComponent(typeof(NU_PlayerMovement))]
public class NU_PlayerInteraction : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;

    [Header("Car Data")]
    [SerializeField] private Transform _vehicleEntryPosition;
    [SerializeField] private bool _isInCar; // ���������, ��������� �� �������� � ����������
    [SerializeField] private Collider _carCollider; // 
    [SerializeField] private IInput _input;

    private void Awake()
    {
        _input = GetComponent<IInput>();
        _cameraController = FindFirstObjectByType<CameraController>();
    }

    private void Update()
    {
        //if (isInCar)
        //{
        //    characterController.enabled = false;
        //    if (currentCar != null)
        //    {
        //        currentCar.HandleInput(); // �������� ����� ��� ��������� �����
        //    }
        //}
        //else
        //{
        //    characterController.enabled = true;
        //}

        //_input.HandleInput();

        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return)) && _carCollider != null)
        {
            if (!_isInCar)
            {
                EnterCar();
            }
            else
            {
                ExitCar();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"������� ������! - {other.name}");
        if (other.CompareTag("Car") && other.TryGetComponent(out IVehicleInfoProvider vehicle))
        {
            //_carCollider = other; // �������� ������ �� ����������
            _vehicleEntryPosition = vehicle.VehicleEntryPosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"������ �� ������ - {other.name}");
        if (other.CompareTag("Car"))
        {
            //_carCollider = null; // ������� ������ �� ����������, ����� �������� �������� �������
            _vehicleEntryPosition = null;
        }
    }

    private void EnterCar()
    {
        if(_carCollider.TryGetComponent(out IInput input))
        {
            _input = input;
            _isInCar = true;
            _cameraController.SetTarget(transform, _carCollider.transform, CameraType.Car);
            // �������� ������������ � ����� � ����
        }
    }

    private void ExitCar()
    {
        _isInCar = false;
        _input = GetComponent<IInput>(); // ��������� ���������� ���������� ����������
        _cameraController.SetTarget(_carCollider.transform, transform, CameraType.Character); /// �������� ������� ������� ������
        /// �������� ������������ � ������ �� ����
    }
}
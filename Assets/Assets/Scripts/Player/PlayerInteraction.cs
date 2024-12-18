using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CarInput _carInput;
    [SerializeField] private bool _isInCar; // ����, �����������, ��������� �� �������� � ����������

    [SerializeField] private CameraController _cameraController;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
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


        if ((Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Return)) && _carInput != null)
        {
            if (!_isInCar)
            {
                EnterCar(_carInput);
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
        if (other.CompareTag("Car"))
        {
            _carInput = other.GetComponent<CarInput>(); // �������� ������ �� CarController
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"������ �� ������ - {other.name}");
        if (other.CompareTag("Car"))
        {
            _carInput = null; // ������� ������, ����� �������� �������� �������
        }
    }

    private void EnterCar(CarInput car)
    {
        _isInCar = true;
        _playerMovement.enabled = false;
        _carInput.enabled = true;
        _cameraController.SetTarget(_carInput.transform);
        // �������� ������������ � ����� � ����
    }

    private void ExitCar()
    {
        _isInCar = false;
        _playerMovement.enabled = true;
        _carInput.enabled = false;
        _cameraController.SetTarget(transform);
        // �������� ������������ � ������ �� ����
    }
}
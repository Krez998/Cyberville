using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private bool _isInCar; // ���������, ��������� �� �������� � ����������
    [SerializeField] private Collider _carCollider; // 

    [SerializeField] private IInput _input;

    [SerializeField] private CameraController _cameraController;

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

        _input.HandleInput();

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
        if (other.CompareTag("Car"))
        {
            _carCollider = other; // �������� ������ �� ����������
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"������ �� ������ - {other.name}");
        if (other.CompareTag("Car"))
        {
            _carCollider = null; // ������� ������ �� ����������, ����� �������� �������� �������
        }
    }

    private void EnterCar()
    {
        if(_carCollider.TryGetComponent(out IInput input))
        {
            _input = input;
            _isInCar = true;
            _cameraController.SetTarget(_carCollider.transform, CameraType.Car);
            // �������� ������������ � ����� � ����
        }
    }

    private void ExitCar()
    {
        _isInCar = false;
        _input = GetComponent<IInput>(); // ��������� ���������� ���������� ����������
        _cameraController.SetTarget(transform, CameraType.Character); /// �������� ������� ������� ������
        /// �������� ������������ � ������ �� ����
    }
}
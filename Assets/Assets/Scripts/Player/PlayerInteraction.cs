using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CarInput _carInput;
    [SerializeField] private bool _isInCar; // Флаг, указывающий, находится ли персонаж в автомобиле

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
        //        currentCar.HandleInput(); // Вызываем метод для обработки ввода
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
        //Debug.Log($"нашлась машина! - {other.name}");
        if (other.CompareTag("Car"))
        {
            _carInput = other.GetComponent<CarInput>(); // Получаем ссылку на CarController
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log($"отошел от машины - {other.name}");
        if (other.CompareTag("Car"))
        {
            _carInput = null; // Очищаем ссылку, когда персонаж покидает триггер
        }
    }

    private void EnterCar(CarInput car)
    {
        _isInCar = true;
        _playerMovement.enabled = false;
        _carInput.enabled = true;
        _cameraController.SetTarget(_carInput.transform);
        // анимация передвижения и входа в авто
    }

    private void ExitCar()
    {
        _isInCar = false;
        _playerMovement.enabled = true;
        _carInput.enabled = false;
        _cameraController.SetTarget(transform);
        // анимация передвижения и выхода из авто
    }
}
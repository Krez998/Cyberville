using System.Collections;
using UnityEngine;

public class VehicleDoors : MonoBehaviour // , IVehicleDoorControl
{
    [SerializeField] private float _openAngle = 60f;
    [SerializeField] private float _openSpeed = 2f;
    [SerializeField] private bool _isOpen = false;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    [Header("Temp Data")]
    [SerializeField] private Transform _door;
    private Vector3 _closedPosition;
    private Vector3 _openPosition;

    private void Start()
    {
        // Запоминаем начальную позицию двери
        if (_door == null)
        {
            Debug.LogError("Дверь не назначена! Проверьте инспектор.");
            return; // Прерываем выполнение метода, если _door не назначена
        }


        _closedPosition = _door.localPosition;
        _openPosition = new Vector3(_closedPosition.x, _closedPosition.y + 1.2f, _closedPosition.z);

        //_closedRotation = transform.rotation;
        //_openRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + _openAngle, transform.eulerAngles.z);
    }

    public void OpenDoor()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            StartCoroutine(DoorAnimation(_openPosition));
        }
    }

    public void CloseDoor()
    {
        if (_isOpen)
        {
            _isOpen = false;
            StartCoroutine(DoorAnimation(_closedPosition));
        }
    }

    private IEnumerator DoorAnimation(Vector3 targetPosition)
    {
        while (Vector3.Distance(_door.transform.localPosition, targetPosition) > 0.01f)
        {
            _door.transform.localPosition = Vector3.MoveTowards(_door.transform.localPosition, targetPosition, Time.deltaTime * _openSpeed);
            yield return null; // Ждем до следующего кадра
        }
        _door.transform.localPosition = targetPosition;
    }

    //private IEnumerator DoorAnimation(Quaternion targetRotation)
    //{
    //    while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _openSpeed);
    //        yield return null; // Ждем до следующего кадра
    //    }
    //    transform.rotation = targetRotation; // Устанавливаем точное значение, чтобы избежать ошибок
    //}
}

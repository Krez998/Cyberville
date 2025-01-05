using System.Collections;
using UnityEngine;

public enum CameraType
{
    Character,
    Car
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraType _cameraType;
    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _distance = 5;
    [SerializeField] private float _smoothSpeed = 1f; // Скорость сглаживания
    [SerializeField] private float _transitionSpeed = 2f; // Скорость перехода камеры

    [SerializeField] float minVerticalAngle = -60;
    [SerializeField] float maxVerticalAngle = 60;

    [SerializeField] Vector2 _framingOffset;

    [SerializeField] bool _invertX;
    [SerializeField] bool _invertY;

    private float _invertXVal;
    private float _invertYVal;

    private Quaternion _targetRotation;
    private float _rotationX;
    private float _rotationY;

    private Transform _source;

    public bool _isInTransition;

    private Coroutine _lerpDistanceCoroutine;

    public void SetTarget(Transform source, Transform target, CameraType cameraType)
    {
        _source = source;
        _followTarget = target;
        _cameraType = cameraType;

        switch (_cameraType)
        {
            case CameraType.Character:
                if (_lerpDistanceCoroutine != null)
                    StopCoroutine(_lerpDistanceCoroutine);
                _lerpDistanceCoroutine = StartCoroutine(LerpDistance(3f));
                //StartCoroutine(LerpPosition());
                break;
            case CameraType.Car:
                if (_lerpDistanceCoroutine != null)
                    StopCoroutine(_lerpDistanceCoroutine);
                _lerpDistanceCoroutine = StartCoroutine(LerpDistance(5f));
                //StartCoroutine(LerpPosition());
                break;
        }
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, _rotationY, 0);

    private IEnumerator LerpDistance(float targetDistance)
    {

        while (Mathf.Abs(_distance - targetDistance) > 0.01f
           /*&& Vector3.Distance(_source.position, _followTarget.position) > 0.01f*/) // Проверяем, достигли ли целевой дистанции
        {
            //Vector3 targetPosition = _followTarget.position + new Vector3(_framingOffset.x, _framingOffset.y) - _targetRotation * new Vector3(0, 0, _distance);
            //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * _transitionSpeed);

            _distance = Mathf.Lerp(_distance, targetDistance, Time.deltaTime * _smoothSpeed);
            yield return null;
        }
        //transform.position = _followTarget.position;
        _distance = targetDistance; // Устанавливаем точное значение, когда достигли

    }

    //private IEnumerator LerpPosition()
    //{
    //    Vector3 sourcePosition = _source.position + new Vector3(_framingOffset.x, _framingOffset.y) - _targetRotation * new Vector3(0, 0, _distance);
    //    Vector3 targetPosition = _followTarget.position + new Vector3(_framingOffset.x, _framingOffset.y) - _targetRotation * new Vector3(0, 0, _distance);

    //    _isInTransition = true;

    //    while(Vector3.Distance(sourcePosition, targetPosition) > 0.5f)
    //    {
    //        yield return null;

    //        Debug.Log(Vector3.Distance(sourcePosition, targetPosition));

    //        targetPosition = _followTarget.position + new Vector3(_framingOffset.x, _framingOffset.y) - _targetRotation * new Vector3(0, 0, _distance);
    //        transform.position = Vector3.Lerp(sourcePosition, targetPosition, Time.deltaTime * _transitionSpeed);

    //    }
    //    transform.position = _followTarget.position;

    //    _isInTransition = false;
    //}

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        _invertXVal = _invertX ? -1 : 1;
        _invertYVal = _invertY ? -1 : 1;

        _rotationX += Input.GetAxis("Mouse Y") * _invertYVal * _rotationSpeed;
        _rotationX = Mathf.Clamp(_rotationX, minVerticalAngle, maxVerticalAngle);

        _rotationY += Input.GetAxis("Mouse X") * _invertXVal * _rotationSpeed;

        _targetRotation = Quaternion.Euler(_rotationX, _rotationY, 0);


        if (!_isInTransition)
            transform.position = _followTarget.position + new Vector3(_framingOffset.x, _framingOffset.y) - _targetRotation * new Vector3(0, 0, _distance);

        transform.rotation = _targetRotation;
    }
}

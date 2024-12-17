using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _distance = 5;
    [SerializeField] float minVerticalAngle = -60;
    [SerializeField] float maxVerticalAngle = 60;

    [SerializeField] Vector2 _framingOffset;

    [SerializeField] bool _invertX;
    [SerializeField] bool _invertY;

    private float _invertXVal;
    private float _invertYVal;

    private float _rotationX;
    private float _rotationY;

    public Quaternion PlanarRotation => Quaternion.Euler(0, _rotationY, 0);

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

        var targetRotation = Quaternion.Euler(_rotationX, _rotationY, 0);


        transform.position = _followTarget.position + new Vector3(_framingOffset.x, _framingOffset.y) - targetRotation * new Vector3(0,0, _distance);
        transform.rotation = targetRotation;
    }
}

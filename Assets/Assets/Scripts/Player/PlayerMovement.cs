using UnityEngine;

public class PlayerMovement : MonoBehaviour, IInput
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed = 500f;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _ySpeed;

    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private Vector3 _groundCheckOffset;
    [SerializeField] private LayerMask _groundLayer;

    private CameraController _cameraController;
    private Animator _animator;
    private CharacterController _characterController;
    private MeeleFighter _meeleFighter;

    private void Awake()
    {
        _cameraController = Camera.main.GetComponent<CameraController>();
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _meeleFighter = GetComponent<MeeleFighter>();
    }

    public void HandleInput()
    {
        // гарантирует что мы не сможем двигаться во время атаки
        if (_meeleFighter.InAction)
        {
            _animator.SetFloat("moveAmount", 0f);
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        CheckGround();

        _ySpeed += _isGrounded ? 0 : Physics.gravity.y * Time.deltaTime;

        var moveInput = new Vector3(h, 0, v).normalized;
        //var moveDirection = _cameraController.PlanarRotation * moveInput;
        var moveDirection = transform.forward * moveAmount;
        var velocity = moveDirection * _moveSpeed;
        velocity.y = _ySpeed;

        //transform.position += moveDirection * _moveSpeed * Time.deltaTime;
        _characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {           
            //var targetRotation = Quaternion.LookRotation(moveDirection);
            var targetRotation = Quaternion.LookRotation(_cameraController.PlanarRotation * moveInput);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
                _rotationSpeed * Time.deltaTime);
        }

        _animator.SetFloat("moveAmount", moveAmount, 0.2f, Time.deltaTime);     
    }

    private void CheckGround()
    {
        Physics.CheckSphere(transform.TransformPoint(_groundCheckOffset), _groundCheckRadius, _groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0,1,0,0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(_groundCheckOffset), _groundCheckRadius);
    }
}

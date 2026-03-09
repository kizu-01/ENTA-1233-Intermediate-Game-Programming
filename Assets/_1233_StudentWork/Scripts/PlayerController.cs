using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    private bool _wasGrounded;  // detect landing
    private bool _jumpRequested;    // detect jumping trigger

    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    [SerializeField] private float speed;

    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [SerializeField] private float jumpPower;
    private int _numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;

    [SerializeField] private Animator _animator;

    // Stored animator parameters as hashes for cleaner code

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int LandHash = Animator.StringToHash("Land");

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();

        HandleJump();
        HandleLanding();
        UpdateAnimatorParameters();
    }

    // Main movement input (gravity, rotation, & movement)

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0f)
            _velocity = -1f;
        else
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;

        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            targetAngle,
            ref _currentVelocity,
            smoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
            _jumpRequested = true;  // Set when Jump is triggered to be handled in Update
    }

    // Transferred Jump logic to HandleJump to avoid double triggers

    private void HandleJump()
    {
        if (!_jumpRequested) return;
        if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;

        _numberOfJumps++;
        _velocity = jumpPower;

        if (IsGrounded())
            _animator.SetTrigger(JumpHash);

        if (_numberOfJumps == 1)
            StartCoroutine(WaitForLanding());

        _jumpRequested = false;
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        _numberOfJumps = 0;
    }

    // Sends gameplay state info to Animator

    private void UpdateAnimatorParameters()
    {
        _animator.SetFloat(SpeedHash, _input.sqrMagnitude);
        _animator.SetBool(IsGroundedHash, IsGrounded());
    }

    private void HandleLanding()
    {
        bool grounded = IsGrounded();

        if (!_wasGrounded && grounded)
            _animator.SetTrigger(LandHash);

        _wasGrounded = grounded;
    }

    private bool IsGrounded() => _characterController.isGrounded;
}
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    private bool _wasGrounded;
    private bool _jumpRequested;

    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    [SerializeField] private float speed;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _gravity = -9.81f;
    private float _velocity;

    [SerializeField] private float jumpPower;
    private int _numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;

    [SerializeField] private Animator _animator;
    [SerializeField] private Health _health;

    // Animator hashes
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int JumpHash = Animator.StringToHash("Jump");
    private static readonly int LandHash = Animator.StringToHash("Land");

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _wasGrounded = IsGrounded();
        if (_health == null) _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (_health != null)
        {
            _health.OnDamaged += HandleDamaged;
            _health.OnDied += HandleDied;
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.OnDamaged -= HandleDamaged;
            _health.OnDied -= HandleDied;
        }
    }

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();

        HandleLanding();
        UpdateAnimatorParameters();
    }

    #region Movement

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0f)
            _velocity = -1f; // stick to ground
        else
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;

        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
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

    #endregion

    #region Jumping

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps)
            return;
        if (_numberOfJumps == 0)
            StartCoroutine(WaitForLanding());

        _numberOfJumps++;
        _velocity += jumpPower;
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Debug.Log("Attacking!");
        _animator?.SetTrigger("Attack");
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        _numberOfJumps = 0; // reset jumps after landing
    }

    #endregion

    #region Animator & Landing

    private void UpdateAnimatorParameters()
    {
        _animator.SetFloat(SpeedHash, _input.sqrMagnitude);
        _animator.SetBool(IsGroundedHash, IsGrounded());
    }

    private void HandleLanding()
    {
        bool grounded = IsGrounded();

        if (!_wasGrounded && grounded)
        {
            _animator.SetTrigger(LandHash);
        }

        _wasGrounded = grounded;
    }

    #endregion

    private bool IsGrounded() => _characterController.isGrounded;



    private void HandleDamaged(DamageInfo info)
    {
        Debug.Log(
            $"[Player] Hit by " +
            $"{info.Source?.name ?? "Unknown"} " +
            $"for {info.Amount} damage. " +
            $"HP: {_health.CurrentHealth}/{_health.MaxHealth}");
        _animator?.SetTrigger("Hit");
    }

    private void HandleDied()
    {
        Debug.Log("[Player] Died!");
        _animator?.SetTrigger("Die");
        _characterController = null;
        enabled = false;

        StartCoroutine(GameOverTransition());
    }

    private IEnumerator GameOverTransition()
    {
        yield return new WaitForSeconds(2);
        GameMgr.Instance.GameOver();
    }
}
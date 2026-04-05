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
    public bool IsAttacking { get; private set; }

    private float _currentVelocity;

    [SerializeField] private float speed;
    [SerializeField] private float gravityMultiplier = 1.5f;
    private float _gravity = -9.81f;
    private float _velocity;

    [SerializeField] private float jumpPower;
    [SerializeField] private PlayerAudioHandler _audioHandler;

    [SerializeField] private Animator _animator;
    [SerializeField] private Health _health;
    [SerializeField] private PlayerAttack _playerAttack;

    private Vector3 _moveDirection;
    [SerializeField] private Transform orientation;

    // Animator hashes
    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");

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
        {
            _velocity = -2f;
        }
        else
        {
            if (_velocity > 0)
            {
                // going up
                _velocity += _gravity * gravityMultiplier * Time.deltaTime;
            }
            else
            {
                // falling
                _velocity += _gravity * gravityMultiplier * 3.0f * Time.deltaTime;
            }
        }
    }

    private void ApplyRotation()
    {
        // If there's a target, smoothly rotate to it
        if (_playerAttack != null && _playerAttack.HasTarget())
        {
            Transform target = _playerAttack.GetTarget();

            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);

                    transform.rotation = Quaternion.Lerp(
                        transform.rotation,
                        targetRotation,
                        10f * Time.deltaTime
                    );
                }
            }

            return;
        }

        // Normal movement rotation
        if (_moveDirection.sqrMagnitude == 0) return;

        float targetAngle = Mathf.Atan2(_moveDirection.x, _moveDirection.z) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0f, targetAngle, 0f),
            15f * Time.deltaTime
        );
    }

    private void ApplyMovement()
    {
        Vector3 finalMove = _moveDirection * speed;
        finalMove.y = _velocity;

        _characterController.Move(finalMove * Time.deltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        Vector3 moveRaw = new(_input.x, 0, _input.y);
        Camera camera = CameraMgr.Instance._mainCamera;
        Vector3 forward = Vector3.Cross(camera.transform.right, Vector3.up);
        Quaternion quat = Quaternion.LookRotation(forward, camera.transform.up);
        _moveDirection = quat * moveRaw;
    }

    #endregion

    #region Jumping

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!IsGrounded()) return;

        _velocity = jumpPower;

        _animator?.SetTrigger("Jump");
        _animator.SetBool("IsJumping", true);
        _audioHandler?.PlayJump();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        IsAttacking = true;

        _animator?.SetTrigger("Attack");
        _playerAttack?.TryAttack();
        _audioHandler?.PlayAttack();

        StartCoroutine(ResetAttackFlag());
    }

    private IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(0.2f);
        IsAttacking = false;
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
            _animator.SetTrigger("Land");
        }

        _wasGrounded = grounded;
        _animator.SetBool("IsJumping", false);
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
        _animator = null;
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
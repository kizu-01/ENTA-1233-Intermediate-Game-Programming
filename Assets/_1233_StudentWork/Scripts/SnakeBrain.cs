using UnityEngine;

public class SnakeBrain : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyStateMachine _stateMachine;

    [SerializeField] private DetectionSystem _detection;
    [SerializeField] private EnemyAnimatorDriver _animatorDriver;
    [SerializeField] private RotateToTarget _rotator;
    [SerializeField] private Health _health;

    [Header("Settings")]
    [SerializeField] private float _attackRange = 2f;

    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _attackDamage = 15;

    public IMover Mover { get; private set; }

    public DetectionSystem Detection => _detection;
    public EnemyAnimatorDriver AnimatorDriver => _animatorDriver;
    public RotateToTarget Rotator => _rotator;
    public ITargetProvider TargetProvider {  get; private set; }

    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public int AttackDamage => _attackDamage;

    private void Awake()
    {
        TargetProvider = GetComponent<ITargetProvider>();
        Mover = GetComponent<IMover>();
        if (_stateMachine == null) _stateMachine = GetComponent<EnemyStateMachine>();
    }

    private void Start()
    {
        _stateMachine.Initialize(new SnakeChaseState(this, _stateMachine));
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

    private void HandleDamaged(DamageInfo info)
    {
        Debug.Log(
            $"[Snake] Hit by " +
            $"{info.Source?.name ?? "Unknown"} " +
            $"for {info.Amount} damage. " +
            $"HP: {_health.CurrentHealth}/{_health.MaxHealth}");
        _animatorDriver?.TriggerHit();
    }

    private void HandleDied()
    {
        if (_stateMachine != null)
            _stateMachine.enabled = false;

        _stateMachine.ChangeState(null);
        if (Mover != null)
        {
            Mover.Stop();
            Mover.SetEnabled(false);
        }

        _animatorDriver.TriggerDie();
        enabled = false;

        Destroy(gameObject, 2f);
    }
}

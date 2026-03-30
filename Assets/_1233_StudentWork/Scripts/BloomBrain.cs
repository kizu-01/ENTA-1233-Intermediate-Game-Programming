using UnityEngine;

public class BloomBrain : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyStateMachine _stateMachine;

    [SerializeField] private DetectionSystem _detection;
    [SerializeField] private EnemyAnimatorDriver _animatorDriver;
    [SerializeField] private RotateToTarget _rotator;
    [SerializeField] private Health _health;
    [SerializeField] private ProjectileWeapon _weapon;
    [SerializeField] public Transform WeaponOrigin;

    [Header("Settings")]
    [SerializeField] private float _attackRange = 10f;

    [SerializeField] private float _stopRange = 8f; // Stay back a bit

    public IMover Mover { get; private set; }

    public DetectionSystem Detection => _detection;
    public EnemyAnimatorDriver AnimatorDriver => _animatorDriver;
    public RotateToTarget Rotator => _rotator;
    public ITargetProvider TargetProvider { get; private set; }

    public ProjectileWeapon Weapon => _weapon;
    public float AttackRange => _attackRange;
    public float StopRange => _stopRange;

    private void Awake()
    {
        TargetProvider = GetComponent<ITargetProvider>();
        Mover = GetComponent<IMover>();
        if (_stateMachine == null) _stateMachine = GetComponent<EnemyStateMachine>();
    }

    private void Start()
    {
        _stateMachine.Initialize(new BloomMoveState(this, _stateMachine));
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
            $"[Bloom] Hit by " +
            $"{info.Source?.name ?? "Unknown"} " +
            $"for {info.Amount} damage. " +
            $"HP: {_health.CurrentHealth}/{_health.MaxHealth}");
        _animatorDriver?.TriggerHit();
    }

    private void HandleDied()
    {
        GameMgr.Instance.AddScore(200);

        if (_stateMachine != null)
            _stateMachine.enabled = false;

        if (Mover != null)
        {
            Mover.Stop();
            Mover.SetEnabled(false);
        }

        _animatorDriver.TriggerDie();
        _weapon.enabled = false;
        enabled = false;

        Destroy(gameObject, 2f);
    }
}
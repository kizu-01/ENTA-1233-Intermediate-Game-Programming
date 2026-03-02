using UnityEngine;

public class SpikeBrain : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EnemyStateMachine _stateMachine;
    [SerializeField] private Health _health;
    [SerializeField] private PatrolMotor _patrolMotor;
    [SerializeField] private ContactDamage _contactDamage;
    [SerializeField] private EnemyAnimatorDriver _animatorDriver;
    [SerializeField] private Transform[] _patrolPoints;
    public Transform[] PatrolPoints => _patrolPoints;

    // Expose Mover publicly like other brains do
    public IMover Mover { get; private set; }

    // Expose animator driver publicly (so states can call _brain.AnimatorDriver)
    public EnemyAnimatorDriver AnimatorDriver => _animatorDriver;

    private void Awake()
    {
        // auto-assign if missing in inspector
        if (_stateMachine == null) _stateMachine = GetComponent<EnemyStateMachine>();
        if (_health == null) _health = GetComponent<Health>();
        if (_patrolMotor == null) _patrolMotor = GetComponent<PatrolMotor>();
        if (_contactDamage == null) _contactDamage = GetComponent<ContactDamage>();
        if (_animatorDriver == null) _animatorDriver = GetComponent<EnemyAnimatorDriver>();

        // Mover should be a component implementing IMover (NavMeshAgentMover implements that)
        Mover = GetComponent<IMover>();
    }

    private void Start()
    {
        // ensure state machine exists before initializing
        if (_stateMachine != null)
            _stateMachine.Initialize(new SpikePatrolState(this, _stateMachine));
        else
            Debug.LogWarning("SpikeBrain: _stateMachine is null — assign an EnemyStateMachine component.");
    }

    private void Update()
    {
        if (_health != null && _health.IsDead) return;

        // Update animator based on mover velocity
        if (_animatorDriver != null && Mover != null)
            _animatorDriver.SetSpeed(Mover.Velocity.magnitude);
    }

    private void OnEnable()
    {
        if (_health != null) _health.OnDied += HandleDied;
    }

    private void OnDisable()
    {
        if (_health != null) _health.OnDied -= HandleDied;
    }

    private void HandleDied()
    {
        if (_patrolMotor != null) _patrolMotor.enabled = false;
        if (_contactDamage != null) _contactDamage.enabled = false;
        if (Mover != null)
        {
            Mover.Stop();
            Mover.SetEnabled(false);
        }

        if (_animatorDriver != null) _animatorDriver.TriggerDie(); 
    }
}
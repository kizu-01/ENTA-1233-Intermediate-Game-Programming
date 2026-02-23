using UnityEngine;

public class SpikeBrain : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private PatrolMotor _patrolMotor;
    [SerializeField] private ContactDamage _contactDamage;
    [SerializeField] private EnemyAnimatorDriver _animatorDriver;

    private IMover _mover;

    private void Awake()
    {
        if (_health == null) _health = GetComponent<Health>();
        if (_patrolMotor == null) _patrolMotor = GetComponent<PatrolMotor>();
        if (_contactDamage == null) _contactDamage = GetComponent<ContactDamage>();
        if (_animatorDriver == null) _animatorDriver = GetComponent<EnemyAnimatorDriver>();
        _mover = GetComponent<IMover>();
    }

    private void Update()
    {
        if (_health != null && _health.IsDead) return;

        // Update animator based on mover velocity
        if (_animatorDriver != null && _mover != null) _animatorDriver.SetSpeed(_mover.Velocity.magnitude);
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
        if (_mover != null)
        {
            _mover.Stop();
            _mover.SetEnabled(false);
        }

        if (_animatorDriver != null) _animatorDriver.TriggerDie();
    }
}

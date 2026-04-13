using UnityEngine;

public class SnakeChaseState : EnemyState
{
    private readonly SnakeBrain _brain;
    private float _nextRepathTime;
    private const float RepathInterval = 0.3f;

    public SnakeChaseState(SnakeBrain brain, EnemyStateMachine machine) : base(machine)
    {
        _brain = brain;
    }

    public override void Tick()
    {
        // Get the player's position
        var target = _brain.TargetProvider.GetTarget();
        if (target == null) return;

        // Check distance & LOS
        if (!_brain.Detection.IsTargetInDetectionRange(target) || !_brain.HasLineOfSight(target))
        {
            _brain.Mover?.Stop();
            _brain.AnimatorDriver.SetSpeed(0);
            return;
        }

        var sqrDistance = 
            (target.position - _brain.transform.position).sqrMagnitude;

        // Switch to attack state if close
        if (sqrDistance <= _brain.AttackRange * _brain.AttackRange)
        {
            Machine.ChangeState(new SnakeAttackState(_brain, Machine));
            return;
        }

        // Repath at interval (do not spam)
        if (Time.time >= _nextRepathTime)
        {
            _brain.Mover?.SetDestination(target.position);
            _nextRepathTime = Time.time + RepathInterval;
        }

        // Update animation
        if (_brain.Mover != null)
            _brain.AnimatorDriver.SetSpeed(_brain.Mover.Velocity.magnitude);
        else
            _brain.AnimatorDriver.SetSpeed(0);
    }
}
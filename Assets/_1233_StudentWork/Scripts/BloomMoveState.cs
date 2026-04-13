using Unity.VisualScripting;
using UnityEngine;

public class BloomMoveState : EnemyState
{
    private readonly BloomBrain _brain;

    public BloomMoveState(BloomBrain brain, EnemyStateMachine machine) : base(machine)
    {
        _brain = brain;
    }

    public override void Tick()
    {
        // 1. Get the player's position
        var target = _brain.TargetProvider.GetTarget();
        if (target == null) return;

        if (!_brain.Detection.IsTargetInDetectionRange(target))
        {
            // Player too far: stop moving
            _brain.Mover?.Stop();
            _brain.AnimatorDriver.SetSpeed(0);
            return;
        }

        var distance = Vector3.Distance(_brain.transform.position, target.position);
        var hasLOS = _brain.Detection.HasLineOfSight(target, _brain.TargetProvider.GetOffset());

        // 2. If we have LOS and are in range, switch to Attack state
        if (hasLOS && distance <= _brain.AttackRange)
        {
            Machine.ChangeState(new BloomAttackState(_brain, Machine));
            return;
        }

        // 3. Move toward target to regain LOS or get in range
        _brain.Mover?.SetDestination(target.position);

        // 4. Update animations based on movement speed
        if (_brain.Mover != null)
            _brain.AnimatorDriver.SetSpeed(_brain.Mover.Velocity.magnitude);
        else
            _brain.AnimatorDriver.SetSpeed(0);
    }
}
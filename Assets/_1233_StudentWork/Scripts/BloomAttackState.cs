using UnityEngine;

public class BloomAttackState : EnemyState
{
    private readonly BloomBrain _brain;

    public BloomAttackState(BloomBrain brain, EnemyStateMachine machine) : base(machine)
    {
        _brain = brain;
    }

    public override void Enter()
    {
        // Stop moving to shoot
        _brain.Mover?.Stop();
        _brain.AnimatorDriver.SetSpeed(0);
    }

    public override void Tick()
    {
        // Check if we still have a target
        var target = _brain.TargetProvider.GetTarget();
        if (target == null)
        {
            Machine.ChangeState(new BloomMoveState(_brain, Machine));
            return;
        }

        // Use squared distance for efficiency
        var sqrDistance =
            (target.position - _brain.transform.position).sqrMagnitude;

        float attackRange = _brain.AttackRange;

        bool hasLOS = _brain.Detection.HasLineOfSight(target);

        // If LOS lost or out of range, then reposition
        if (!hasLOS || sqrDistance > attackRange * attackRange)
        {
            Machine.ChangeState(new BloomMoveState(_brain, Machine));
            return;
        }

        // Face the target
        _brain.Rotator.FacePosition(target.position);

        // Fire if ready
        if (_brain.Weapon.CanFire)
        {
            _brain.AnimatorDriver.TriggerAttack();
            _brain.Weapon.Fire(target.position);
        }
    }
}

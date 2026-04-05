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
        _brain.Mover?.SetEnabled(false);
        _brain.AnimatorDriver.SetSpeed(0);
    }

    public override void Tick()
    {
        // 1. Check if we still have a target
        var target = _brain.TargetProvider.GetTarget();
        var targetPos = _brain.TargetProvider.GetTargetPosition();
        if (target == null)
        {
            Machine.ChangeState(new BloomMoveState(_brain, Machine));
            return;
        }

        var distance = Vector3.Distance(_brain.transform.position, target.position);
        var hasLOS = _brain.Detection.HasLineOfSight(target, _brain.TargetProvider.GetOffset());

        // 2. If LOS is lost or we are out of range, go back to Move state
        if (!_brain.Detection.IsTargetInDetectionRange(target) || !hasLOS || distance > _brain.AttackRange)
        {
            Machine.ChangeState(new BloomMoveState(_brain, Machine));
            return;
        }

        // 3. Face the player and shoot if weapon is ready
        _brain.Rotator.FacePosition(targetPos);
        if (_brain.Weapon.CanFire)
        {
            _brain.AnimatorDriver.TriggerAttack();
            _brain.GetComponent<EnemyAudioHandler>()?.PlayAttack();
            _brain.Weapon.Fire(targetPos);
        }
    }

    public override void Exit()
    {
        _brain.Mover?.SetEnabled(true);
    }
}
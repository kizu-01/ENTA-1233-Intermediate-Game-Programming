using UnityEngine;

public class SnakeChaseState : EnemyState
{
    private readonly SnakeBrain _brain;

    public SnakeChaseState(SnakeBrain brain, EnemyStateMachine machine) : base(machine)
    {
        _brain = brain;
    }

    public override void Tick()
    {
        // 1. Get the player's position
        var target = _brain.TargetProvider.GetTarget();
        if (target == null) return;

        // 2. Tell the mover to go there
        _brain.Mover?.SetDestination(target.position);

        // 3. Update animations based on movement speed
        if (_brain.Mover != null)
            _brain.AnimatorDriver.SetSpeed(_brain.Mover.Velocity.magnitude);
        else
            _brain.AnimatorDriver.SetSpeed(0);

        // 4. If we are close enough, switch to Attack state
        var distance = Vector3.Distance(_brain.transform.position, target.position);
        if (distance <= _brain.AttackRange) Machine.ChangeState(new SnakeAttackState(_brain, Machine));
    }
}
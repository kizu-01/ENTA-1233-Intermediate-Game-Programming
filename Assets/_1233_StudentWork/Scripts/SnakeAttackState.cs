using UnityEngine;

public class SnakeAttackState : EnemyState
{
    private readonly SnakeBrain _brain;
    private float _exitTime;

    public SnakeAttackState(SnakeBrain brain, EnemyStateMachine machine) : base(machine)
    {
        _brain = brain;
    }

    public override void Enter()
    {
        // Stop moving and trigger the attack animation
        _brain.Mover?.Stop();
        _brain.AnimatorDriver.SetSpeed(0);
        _brain.AnimatorDriver.TriggerAttack();
        _brain.GetComponent<EnemyAudioHandler>()?.PlayAttack();

        // Calculate when we can leave this state
        _exitTime = Time.time + _brain.AttackCooldown;

        // Apply damage immediately (simplified)
        ApplyMeleeDamage();
    }

    public override void Tick()
    {
        // Keep facing the player during the attack
        var target = _brain.TargetProvider.GetTarget();
        if (target == null) return;

        var targetPos = _brain.TargetProvider.GetTargetPosition();
        _brain.Rotator.FacePosition(targetPos);

        // Return to chase state once the cooldown is over
        if (Time.time >= _exitTime)
        {
            Machine.ChangeState(new SnakeChaseState(_brain, Machine));
        }
    }

    private void ApplyMeleeDamage()
    {
        var target = _brain.TargetProvider.GetTarget();
        if (target == null) return;

        var sqrDistance = (target.position - _brain.transform.position).sqrMagnitude;

        // Final check to see if target is still in range
        if (sqrDistance <= (_brain.AttackRange + 0.5f) * (_brain.AttackRange + 0.5f))
        {
            var receiver = target.GetComponentInChildren<IDamageReceiver>();
            if (receiver != null)
                receiver.ApplyDamage(
                    new DamageInfo
                    {
                        Amount = _brain.AttackDamage,
                        Source = _brain.gameObject,
                        HitPoint = target.position,
                        HitNormal = Vector3.up
                    });
        }
    }
}

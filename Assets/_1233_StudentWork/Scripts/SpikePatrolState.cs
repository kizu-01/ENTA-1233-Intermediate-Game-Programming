using UnityEngine;

public class SpikePatrolState : EnemyState
{
    private readonly SpikeBrain _brain;

    private int _currentIndex;
    private float _waitTimer;
    private const float WaitTime = 0.5f; // optional wait at each point

    public SpikePatrolState(SpikeBrain brain, EnemyStateMachine machine)
        : base(machine)
    {
        _brain = brain;
    }

    public override void Enter()
    {
        _currentIndex = 0;
        MoveToCurrentPoint();
    }

    public override void Tick()
    {
        if (_brain.PatrolPoints == null || _brain.PatrolPoints.Length == 0)
            return;

        // Update animation speed
        if (_brain.Mover != null)
            _brain.AnimatorDriver.SetSpeed(_brain.Mover.Velocity.magnitude);

        // Check if we reached destination
        if (_brain.Mover != null && _brain.Mover.IsAtDestination)
        {
            _waitTimer += Time.deltaTime;

            if (_waitTimer >= WaitTime)
            {
                AdvanceIndex();
                MoveToCurrentPoint();
                _waitTimer = 0f;
            }
        }
    }

    private void MoveToCurrentPoint()
    {
        var point = _brain.PatrolPoints[_currentIndex];
        if (point != null)
        {
            _brain.Mover?.SetDestination(point.position);
        }
    }

    private void AdvanceIndex()
    {
        _currentIndex++;

        if (_currentIndex >= _brain.PatrolPoints.Length)
            _currentIndex = 0; // loop back
    }
}
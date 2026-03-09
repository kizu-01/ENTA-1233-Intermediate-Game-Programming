using UnityEngine;

public abstract class EnemyState
{
    protected EnemyStateMachine Machine;
    
    public EnemyState(EnemyStateMachine machine)
    {
        Machine = machine;
    }

    public virtual void Enter()
    {
    }

    public virtual void Tick()
    {
    }

    public virtual void Exit()
    {
    }
}

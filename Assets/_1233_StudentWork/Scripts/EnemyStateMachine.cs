using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    private EnemyState _currentState;

    private void Update()
    {
        _currentState?.Tick();
    }

    public void Initialize(EnemyState initialState)
    {
        _currentState = initialState;
        _currentState.Enter();
    }

    public void ChangeState(EnemyState newState)
    {
        if (newState == null) return;

        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}

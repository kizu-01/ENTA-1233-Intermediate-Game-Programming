using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshAgentMover : MonoBehaviour, IMover
{
    [SerializeField] private NavMeshAgent _agent;

    // public getters for agent related information
    public Vector3 Velocity => _agent.velocity;
    public bool HasPath => _agent.hasPath;
    public float RemainingDistance => _agent.remainingDistance;

    public bool IsAtDestination =>
        !_agent.pathPending &&
        _agent.remainingDistance <= _agent.stoppingDistance;

    private void Awake()
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 worldPos)
    {
        _agent?.SetDestination(worldPos);
    }

    public void Stop()
    {
        if (_agent == null) return;

        if (!_agent.isActiveAndEnabled || !_agent.isOnNavMesh)
            return;

        _agent.isStopped = true;
        _agent.ResetPath();
        _agent.velocity = Vector3.zero;
    }

    public void Resume()
    {
        if (_agent == null) return;
        _agent.isStopped = false;
    }

    public void SetEnabled(bool enabled)
    {
        if (_agent == null) return;
        _agent.enabled = enabled;
    }
}

using UnityEngine;

[RequireComponent (typeof(NavMeshAgentMover))]
public class SetDestinationOnStart : MonoBehaviour
{
    [SerializeField] private NavMeshAgentMover _agent;
    [SerializeField] private Transform _destination;

    private void Start()
    {
        _agent.SetDestination(_destination.position);
    }
}

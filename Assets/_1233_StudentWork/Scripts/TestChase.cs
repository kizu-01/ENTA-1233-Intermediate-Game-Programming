using UnityEngine;
using UnityEngine.AI;

public class TestChase : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _chaseRange = 10f;

    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_target == null) return;

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _chaseRange)
        {
            _agent.SetDestination(_target.position);
        }
        else
        {
            _agent.ResetPath(); // stop moving if out of range
        }
    }
}
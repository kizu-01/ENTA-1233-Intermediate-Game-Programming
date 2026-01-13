using UnityEngine;

public class PhysicsForces : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _force; // [x, y, z]
    [SerializeField] private bool _continuous;
    [SerializeField] private ForceMode _forceMode;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        if (!_continuous)
            _rigidbody.AddForce(_force, _forceMode);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_continuous)
            _rigidbody.AddForce(_force, _forceMode);
    }
}

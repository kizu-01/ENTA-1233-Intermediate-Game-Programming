using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private bool _useGravity;

    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int explosionDamage = 20;
    [SerializeField] private LayerMask damageLayers;
    [SerializeField] private float pushDistance = 2f;

    private Rigidbody _rb;
    private GameObject _source;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = _useGravity;
    }

    private void OnCollisionEnter(Collision collision)
    {

        // Don't hit the source
        if (collision.gameObject == _source) return;

        // Check if we hit something damageable
        var damageReceiver = collision.gameObject.GetComponentInParent<IDamageReceiver>();
        if (damageReceiver != null)
        {
            var info = new DamageInfo
            {
                Amount = _damage,
                Source = _source,
                HitPoint = collision.contacts[0].point,
                HitNormal = collision.contacts[0].normal,
            };
            damageReceiver.ApplyDamage(info);
        }

        if (impactEffect != null)
        {
            Instantiate(
                impactEffect,
                collision.contacts[0].point,
                Quaternion.identity
            );
        }

        Vector3 hitPoint = collision.contacts[0].point;

        // Spawn explosion VFX
        if (impactEffect != null)
        {
            Instantiate(
                impactEffect,
                hitPoint,
                Quaternion.LookRotation(collision.contacts[0].normal)
            );
        }

        // Do explosion logic
        Explode(hitPoint);
        // Destroy on impact
        Destroy(gameObject);
    }

    public void Launch(Vector3 direction, GameObject source)
    {
        _source = source;
        _rb.linearVelocity = direction.normalized * _speed;
        transform.forward = direction;
        Destroy(gameObject, _lifetime); // Simple destruction for now
    }

    void Explode(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, explosionRadius, damageLayers);

        foreach (var hit in hits)
        {
            // damage
            var receiver = hit.GetComponentInParent<IDamageReceiver>();
            if (receiver != null)
            {
                receiver.ApplyDamage(new DamageInfo
                {
                    Amount = explosionDamage,
                    Source = _source,
                    HitPoint = position,
                    HitNormal = Vector3.up
                });
            }

            // push back / knockback effect
            var mover = hit.GetComponentInParent<IMover>();
            if (mover != null)
            {
                Vector3 dir = (hit.transform.position - position).normalized;

                float dist = Vector3.Distance(position, hit.transform.position);
                float force = Mathf.Lerp(pushDistance, 0f, dist / explosionRadius);

                Vector3 pushPos = hit.transform.position + dir * force;

                mover.SetDestination(pushPos);
            }
        }
    }

    public void LaunchWithVelocity(Vector3 velocity, GameObject source)
    {
        _source = source;
        if (float.IsNaN(velocity.x) || float.IsNaN(velocity.y) || float.IsNaN(velocity.z))
        {
            Debug.LogWarning("Projectile velocity invalid. Aborting shot.");
            return;
        }
        _rb.linearVelocity = velocity;
        if (velocity.sqrMagnitude > 0.001f)
            transform.forward = velocity;
        _rb.useGravity = true; // Force gravity for arc shots
        Destroy(gameObject, _lifetime);
    }
}
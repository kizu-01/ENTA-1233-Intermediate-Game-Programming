using UnityEngine;

public class ProjectileWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _arcHeight = 2f;

    private float _nextFireTime;

    public bool CanFire => Time.time >= _nextFireTime;

    public void Fire(Vector3 targetPosition)
    {
        if (!CanFire) return;

        _nextFireTime = Time.time + 1f / _fireRate;

        // Direct fire by default, or could be configured for arc
        var direction = (targetPosition - _muzzle.position).normalized;
        SpawnProjectile(direction);
    }

    public void Fire(Vector3 direction, bool useDirection)
    {
        if (!CanFire) return;

        _nextFireTime = Time.time + 1f / _fireRate;
        SpawnProjectile(direction);
    }

    public void FireArc(Vector3 targetPosition)
    {
        if (!CanFire) return;

        _nextFireTime = Time.time + 1f / _fireRate;

        var velocity = CalculateArcVelocity(_muzzle.position, targetPosition, _arcHeight);
        if (float.IsNaN(velocity.x) || float.IsNaN(velocity.y) || float.IsNaN(velocity.z))
            return;
        var projectile = Instantiate(_projectilePrefab, _muzzle.position, _muzzle.rotation);
        projectile.LaunchWithVelocity(velocity, gameObject);
    }

    private void SpawnProjectile(Vector3 direction)
    {
        var projectile = Instantiate(_projectilePrefab, _muzzle.position,
            Quaternion.LookRotation(direction));
        projectile.Launch(direction, gameObject);
    }

    private Vector3 CalculateArcVelocity(Vector3 start, Vector3 end, float height)
    {
        var displacementY = end.y - start.y;
        var displacementXZ = new Vector3(end.x - start.x, 0, end.z - start.z);
        var gravity = Physics.gravity.y;

        var time = Mathf.Sqrt(-2 * height / gravity) +
            Mathf.Sqrt(2 * (displacementY - height) / gravity);
        var velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        var velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }
}
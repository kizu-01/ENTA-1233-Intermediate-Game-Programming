using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 12f;
    [SerializeField] private LayerMask enemyLayer;

    private float nextAttackTime;

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        TryAttack();
    }

    public void TryAttack()
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        Transform target = FindClosestEnemy();

        if (target != null)
        {
            Vector3 direction = (target.position - firePoint.position).normalized;
            FireProjectile(direction);
        }
        else
        {
            FireProjectile(firePoint.forward);
        }
    }

    Transform FindClosestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Vector3 directionToEnemy = hit.transform.position - transform.position;

            float dot = Vector3.Dot(transform.forward, directionToEnemy.normalized);

            if (dot < 0.5f)
                continue;

            float distance = directionToEnemy.sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = hit.transform;
            }
        }

        return closest;
    }

    void FireProjectile(Vector3 direction)
    {
        GameObject projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        projectile.GetComponent<Projectile>()
            .Launch(direction, gameObject);
    }
}
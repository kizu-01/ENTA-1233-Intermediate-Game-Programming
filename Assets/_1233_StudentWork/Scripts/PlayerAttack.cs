using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 12f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;

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

        // ALWAYS get nearest enemy (no lock-on)
        Transform target = FindClosestEnemy();

        Vector3 direction;

        if (target != null)
        {
            Vector3 targetPoint = GetTargetPoint(target);

            // Instantly face the target
            // FaceTargetInstant(targetPoint);

            // Shoot directly at target
            direction = (targetPoint - firePoint.position).normalized;
        }
        else
        {
            // fallback if no enemy
            direction = transform.forward;
        }

        FireProjectile(direction);
    }

    Transform FindClosestEnemy()
    {
        LayerMask combinedMask = enemyLayer | obstacleLayer;

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        Transform bestTarget = null;
        float bestScore = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Vector3 targetPoint = GetTargetPoint(hit.transform);
            Vector3 directionToEnemy = targetPoint - firePoint.position;
            float distance = directionToEnemy.magnitude;
            Vector3 dirNormalized = directionToEnemy.normalized;

            // 2. Fixed Raycast: Check if the first thing hit is not the enemy
            if (Physics.Raycast(firePoint.position, dirNormalized, out RaycastHit rayHit, attackRange, combinedMask))
            {
                // If ray hits an obstacle before it hits enemy collider, skip
                if (rayHit.collider.gameObject != hit.gameObject)
                    continue;
            }

            // 3. Focus heavily towards distance first
            float dot = Vector3.Dot(transform.forward, dirNormalized);

            // Target enemies in front
            float score = distance - (dot * 2.0f);

            if (score < bestScore)
            {
                bestScore = score;
                bestTarget = hit.transform;
            }
        }

        return bestTarget;
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

    Vector3 GetTargetPoint(Transform target)
    {
        Collider col = target.GetComponent<Collider>();

        if (col != null)
        {
            // Aim slightly above center to avoid hitting feet
            return col.bounds.center + Vector3.up * (col.bounds.size.y * 0.3f);
        }

        return target.position + Vector3.up * 1.5f;
    }

    void FaceTargetInstant(Vector3 targetPoint)
    {
        Vector3 direction = targetPoint - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        transform.rotation = Quaternion.LookRotation(direction);
    }

    public bool HasTarget()
    {
        return FindClosestEnemy() != null;
    }
    public Transform GetTarget()
    {
        return FindClosestEnemy();
    }
}
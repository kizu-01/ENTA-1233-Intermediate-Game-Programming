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

        Vector3 direction;

        if (target != null)
        {
            Vector3 targetPoint = GetTargetPoint(target);

            StartCoroutine(SmoothFaceTarget(targetPoint));

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
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        Transform bestTarget = null;
        float bestScore = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            Vector3 directionToEnemy = hit.transform.position - transform.position;
            float distance = directionToEnemy.magnitude;

            Vector3 dirNormalized = directionToEnemy.normalized;
            float dot = Vector3.Dot(transform.forward, dirNormalized);

            // ignore enemies mostly behind the player
            if (dot < 0.3f)
                continue;

            // scoring: prioritize enemies in the center of the screen
            float score = distance * (1.5f - dot);

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
            return col.bounds.center;

        return target.position + Vector3.up * 1.2f;
    }

    void FaceTarget(Vector3 targetPoint)
    {
        Vector3 direction = targetPoint - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            20f * Time.deltaTime
        );
    }

    private IEnumerator SmoothFaceTarget(Vector3 targetPoint)
    {
        float timer = 0f;

        while (timer < 0.15f) // short smooth turn
        {
            FaceTarget(targetPoint);
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
using UnityEngine;

public class EnemyTargetIndicator : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;
    private GameObject currentIndicator;

    private void Update()
    {
        var playerAttack = FindFirstObjectByType<PlayerAttack>();

        if (playerAttack == null) return;

        if (playerAttack.GetTarget() == transform)
        {
            if (currentIndicator == null)
            {
                currentIndicator = Instantiate(indicatorPrefab, transform);
                currentIndicator.transform.localPosition = new Vector3(0, 2f, 0);
            }
        }
        else
        {
            if (currentIndicator != null)
            {
                Destroy(currentIndicator);
                currentIndicator = null;
            }
        }
    }
}
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using System.Collections;

public class TrainingDummyBrain : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private EnemyAnimatorDriver _animatorDriver;
    [SerializeField] private float _resetDelay = 2f;
    [SerializeField] private bool _autoReset = true;

    private void Awake()
    {
        if (_health == null) _health = GetComponent<Health>();
        if (_animatorDriver == null)
            _animatorDriver = GetComponent<EnemyAnimatorDriver>();
    }

    private void OnEnable()
    {
        if (_health != null)
        {
            _health.OnDamaged += HandleDamaged;
            _health.OnDied += HandleDied;
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.OnDamaged -= HandleDamaged;
            _health.OnDied -= HandleDied;
        }
    }

    private void HandleDamaged(DamageInfo info)
    {
        Debug.Log(
            $"[Dummy] Hit by " +
            $"{info.Source?.name ?? "Unknown"} " +
            $"for {info.Amount} damage. " +
            $"HP: {_health.CurrentHealth}/{_health.MaxHealth}");
        _animatorDriver?.TriggerHit();
    }

    private void HandleDied()
    {
        Debug.Log("[Dummy] Died!");
        Debug.Log($"HandleDied called at {Time.time}, activeSelf: {gameObject.activeSelf}");

        _animatorDriver.TriggerDie();

        if (_autoReset) Invoke(nameof(ResetDummy), _resetDelay);
    }

    private void ResetDummy()
    {
        _health.ResetHealth();
    }
}

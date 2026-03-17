using System;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

/// <summary>
///     Manages health values and state for an entity.
///     Emits events for other systems (VFX, Audio, UI) to listen to.
/// </summary>
public class Health : MonoBehaviour, IDamageReceiver
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private bool _isInvulnerable;

    public int CurrentHealth { get; private set; }

    public int MaxHealth => _maxHealth;

    public bool IsDead {  get; private set; }

    private void Awake()
    {
        ResetHealth();
    }

    // Events for other systems to subscribe to
    public event Action<DamageInfo> OnDamaged;
    public event Action OnDied;
    public event Action OnHealed;
    public event Action OnReset;

    public void ResetHealth()
    {
        CurrentHealth = _maxHealth;
        IsDead = false;
        OnReset?.Invoke();
    }

    public void ApplyDamage(DamageInfo info)
    {
        Debug.Log($"{gameObject.name} took {info.Amount} damage");
        if (IsDead || _isInvulnerable) return;

        CurrentHealth -= info.Amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _maxHealth);

        OnDamaged?.Invoke(info);

        if (CurrentHealth <= 0) Die();
    }

    public void Heal(int amount)
    {
        if(IsDead) return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, _maxHealth);
        OnHealed?.Invoke();
    }

    private void Die()
    {
        IsDead = true;
        OnDied?.Invoke();
    }

    // For Training Dummy or special cases
    public void SetInvulnerable(bool invulnerable)
    {
        _isInvulnerable = invulnerable;
    }
}

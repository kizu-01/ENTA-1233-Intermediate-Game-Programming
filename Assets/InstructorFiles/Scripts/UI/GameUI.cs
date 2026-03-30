using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// In game HUD shown when not paused
/// </summary>
public class GameUI : MenuBase
{
    public override GameMenus MenuType()
    {
        return GameMenus.InGameUI;
    }

    [SerializeField] private Image _healthFillImage;

    private Health _playerHealth;

    private void OnEnable()
    {
        if (PlayerMgr.Instance == null)
        {
            Debug.LogError("GameUI: PlayerMgr is null.");
            return;
        }

        // if player was set already
        if (PlayerMgr.Instance.HasSpawnedPlayer && PlayerMgr.Instance.PlayerObject != null)
        {
            HandlePlayerAssigned(PlayerMgr.Instance.PlayerObject);
        }
        else
        {
            PlayerMgr.Instance.OnPlayerAssigned += HandlePlayerAssigned;
        }
    }

    private void OnDisable()
    {
        if (PlayerMgr.Instance != null) PlayerMgr.Instance.OnPlayerAssigned -= HandlePlayerAssigned;
    }

    private void HandlePlayerAssigned(GameObject playerObject)
    {
        if (playerObject == null)
        {
            RefreshHealthBar(null);
            return;
        }

        _playerHealth = playerObject.GetComponent<Health>();
        if (_playerHealth == null)
        {
            Debug.LogError("GameUI: Player object does not have a Health component.");
            return;
        }

        _playerHealth.OnHealthChanged += RefreshHealthBar;
        RefreshHealthBar(_playerHealth);
    }

    private void RefreshHealthBar(Health health)
    {
        Debug.Log("UI updating: " + (health != null ? health.NormalizedHealth : 0f));

        if (_healthFillImage == null) return;

        _healthFillImage.fillAmount = health != null ? health.NormalizedHealth : 0f;
    }
}

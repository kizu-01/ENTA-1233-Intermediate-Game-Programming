using System;
using UnityEngine;

/// <summary>
/// A singleton for communicating with the player object when it exists
/// </summary>
public class PlayerMgr : Singleton<PlayerMgr>
{
    [SerializeField] private GameObject _playerPrefab;

    public GameObject PlayerObject { get; private set; }
    public bool HasSpawnedPlayer => _playerPrefab != null;
    public event Action<GameObject> OnPlayerAssigned;

    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        if (PlayerObject)
        {
            Debug.LogError("Player already spawned!");
            return;
        }

        PlayerObject = Instantiate(_playerPrefab, position, rotation);
        OnPlayerAssigned?.Invoke(PlayerObject);
        Debug.Log("Player spawned");
    }

    /// <summary>
    /// Handles the player using the pause input action
    /// TODO move to player input handler separate from player controller
    /// </summary>
    public void PauseInput()
    {
        // Run pause from game manager
        GameMgr.Instance.PauseGameToggle();
    }

    public void DebugAssignAsPlayer(GameObject existingPlayer)
    {
        PlayerObject = existingPlayer;
        OnPlayerAssigned?.Invoke(PlayerObject);
    }
}
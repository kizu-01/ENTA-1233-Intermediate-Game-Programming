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

    public void SpawnPlayer(Vector3 position, Quaternion rotation)
    {
        if (PlayerObject)
        {
            Debug.LogError("Player already spawned!");
            return;
        }

        PlayerObject = Instantiate(_playerPrefab, position, rotation);
        Debug.Log("Player spawned");
    }
}
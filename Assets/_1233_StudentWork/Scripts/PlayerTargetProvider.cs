using UnityEngine;

public class PlayerTargetProvider : MonoBehaviour, ITargetProvider
{
    [SerializeField] private Vector3 _offset = new(0, 1f, 0);

    public bool HasTarget => PlayerMgr.Instance != null && PlayerMgr.Instance.HasSpawnedPlayer;

    public Transform GetTarget()
    {
        if (PlayerMgr.Instance == null) return null;

        if (!PlayerMgr.Instance.HasSpawnedPlayer) return null;

        if (PlayerMgr.Instance.PlayerObject == null) return null;

        return PlayerMgr.Instance.PlayerObject.transform;
    }

    public Vector3 GetTargetPosition()
    {
        var target = GetTarget();

        if (target != null)
            return target.position + _offset;

        return transform.position;
    }
    public Vector3 GetOffset()
    { 
        return _offset; 
    }
}

using UnityEngine;
public class LevelCompleteTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        GameMgr.Instance.SaveBestScore();
        GameMgr.Instance.LevelComplete();
    }
}

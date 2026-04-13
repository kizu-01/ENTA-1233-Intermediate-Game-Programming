using TMPro;
using UnityEngine;

public class LevelSelectObject : MonoBehaviour
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    private void OnEnable()
    {
        string key = $"BestScore_Level_{_levelIndex}";
        float best = PlayerPrefs.GetFloat(key, 0);

        _bestScoreText.text = $"Best: {Mathf.RoundToInt(best)}";
    }

    public void SelectLevel()
    {
        LevelMgr.Instance.SetCurrentLevel(_levelIndex);
        SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
    }
}
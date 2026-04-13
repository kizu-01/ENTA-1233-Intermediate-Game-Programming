using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Game over screen
/// Allows for quitting or retrying
/// </summary>
public class LevelComplete : MenuBase
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;

    [Header("Buttons")]
    [SerializeField] private Button _nextLevelButton;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        float score = GameMgr.Instance.Score;
        int levelIndex = LevelMgr.Instance.CurrentLevelIndex;

        int highestUnlocked = PlayerPrefs.GetInt("HighestUnlockedLevel", 0);
        if (levelIndex == highestUnlocked)
        {
            // If Level 1 finished, unlock Level 2
            PlayerPrefs.SetInt("HighestUnlockedLevel", levelIndex + 1);
            PlayerPrefs.Save();
        }

        string key = $"BestScore_Level_{levelIndex}";
        float bestScore = PlayerPrefs.GetFloat(key, 0);

        if (score > bestScore)
        {
            PlayerPrefs.SetFloat(key, score);
            bestScore = score;
        }
            
        _scoreText.text = $"Score: {Mathf.RoundToInt(score)}";
        _bestScoreText.text = $"Best: {Mathf.RoundToInt(bestScore)}";

        SetupNextButton(levelIndex);
    }

    private void UnlockNextLevel(int currentLevelIndex)
    {
        // Get the highest level currently unlocked (default is 0 for Level 1)
        int highestUnlocked = PlayerPrefs.GetInt("HighestUnlockedLevel", 0);

        // If finished highest level, unlock the next one
        if (currentLevelIndex == highestUnlocked)
        {
            PlayerPrefs.SetInt("HighestUnlockedLevel", currentLevelIndex + 1);
            PlayerPrefs.Save();
        }
    }

    private void SetupNextButton(int levelIndex)
    {
        int lastLevelIndex = LevelMgr.Instance.LevelSceneNames.Length - 1;

        // Clear old listeners
        _nextLevelButton.onClick.RemoveAllListeners();

        var buttonText = _nextLevelButton.GetComponentInChildren<TextMeshProUGUI>();

        if (levelIndex >= lastLevelIndex)
        {
            // If Last Level: Play Again
            if (buttonText != null)
                buttonText.text = "Play Again";

            _nextLevelButton.onClick.AddListener(() =>
            {
                SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
            });
        }
        else
        {
            // Normal Next Level
            if (buttonText != null)
                buttonText.text = "Next Level";

            _nextLevelButton.onClick.AddListener(() =>
            {
                LevelMgr.Instance.NextLevel();
                SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
            });
        }
    }

    public override GameMenus MenuType()
    {
        return GameMenus.LevelCompleteMenu;
    }

    public void ButtonNextLevel()
    {
        LevelMgr.Instance.NextLevel();
        SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
    }

    public void ButtonMainMenu()
    {
        LevelMgr.Instance.ResetLevels();
        SceneMgr.Instance.LoadScene(GameScenes.MainMenu, GameMenus.MainMenu);
    }
}

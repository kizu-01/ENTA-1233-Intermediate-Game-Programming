using TMPro;
using UnityEngine;


/// <summary>
/// Game over screen
/// Allows for quitting or retrying
/// </summary>
public class GameOver : MenuBase
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioMgr.Instance.PlayMusic(AudioMgr.MusicTypes.GameOver, 2);

        if (_scoreText != null)
        {
            float score = GameMgr.Instance.Score;
            _scoreText.text = $"Score: {Mathf.RoundToInt(score)}";
        }
    }

    public override GameMenus MenuType()
    {
        return GameMenus.GameOverMenu;
    }

    public void ButtonRetry()
    {
        SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
    }

    public void ButtonMainMenu()
    {
        LevelMgr.Instance.ResetLevels();
        SceneMgr.Instance.LoadScene(GameScenes.MainMenu, GameMenus.MainMenu);
    }
}

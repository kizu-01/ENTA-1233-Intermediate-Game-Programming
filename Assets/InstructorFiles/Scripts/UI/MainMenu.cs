using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// The main menu when starting the game
/// The simple entry point after the game loads and return point if exiting gameplay
/// </summary>
public class MainMenu : MenuBase
{
    [SerializeField] private Button _startButton;
    
    public override GameMenus MenuType()
    {
        return GameMenus.MainMenu;
    }

    private void OnEnable()
    {
        AudioMgr.Instance.PlayMusic(AudioMgr.MusicTypes.MainMenu, 1);
    }

    public void ButtonStart()
    {
        LevelMgr.Instance.ResetLevels();
        SceneMgr.Instance.LoadScene(GameScenes.Gameplay, GameMenus.InGameUI);
    }

    public void ButtonSelectLevel()
    {
        UIMgr.Instance.ShowMenu(GameMenus.LevelSelectMenu);
    }

    public void ButtonSettings()
    {
        UIMgr.Instance.ShowMenu(GameMenus.SettingsMenu);

        Settings settings = FindFirstObjectByType<Settings>();
        if (settings != null)
            settings.SetPreviousMenu(GameMenus.MainMenu);
    }

    public void ButtonQuit()
    {
        Application.Quit();

    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}

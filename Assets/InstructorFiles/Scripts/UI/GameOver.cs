using UnityEngine;


/// <summary>
/// Game over screen
/// Allows for quitting or retrying
/// </summary>
public class GameOver : MenuBase
{
    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioMgr.Instance.PlayMusic(AudioMgr.MusicTypes.GameOver, 2);
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
        SceneMgr.Instance.LoadScene(GameScenes.MainMenu, GameMenus.MainMenu);
    }
}

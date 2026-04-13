using UnityEngine;

public class PauseMenu : MenuBase
{
    public override GameMenus MenuType() => GameMenus.PauseMenu;

    public void OnResume()
    {
        GameMgr.Instance.PauseGameToggle();
    }

    public void OnSettings()
    {
        UIMgr.Instance.HideMenu(GameMenus.PauseMenu);
        UIMgr.Instance.ShowMenu(GameMenus.SettingsMenu);

        Settings settings = FindFirstObjectByType<Settings>();
        if (settings != null)
            settings.SetPreviousMenu(GameMenus.PauseMenu);
    }

    public void OnExitLevel()
    {
        GameMgr.Instance.ResetGameState(); // fully reset everything
        SceneMgr.Instance.LoadScene(GameScenes.MainMenu, GameMenus.MainMenu);
    }
}
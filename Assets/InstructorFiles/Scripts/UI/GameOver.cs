/// <summary>
/// Game over screen
/// Allows for quitting or retrying
/// </summary>
public class GameOver : MenuBase
{
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

    private void OnEnable()
    {
        AudioMgr.Instance.PlayMusic(AudioMgr.MusicTypes.GameOver, 2);
    }
}

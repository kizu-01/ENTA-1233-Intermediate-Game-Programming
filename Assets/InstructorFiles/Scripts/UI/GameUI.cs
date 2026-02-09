using System.Runtime.CompilerServices;


/// <summary>
/// In game HUD shown when not paused
/// </summary>
public class GameUI : MenuBase
{
    public override GameMenus MenuType()
    {
        return GameMenus.InGameUI;
    }

    private void OnEnable()
    {
        AudioMgr.Instance.PlayMusic(AudioMgr.MusicTypes.Gameplay, 1);
    }
}

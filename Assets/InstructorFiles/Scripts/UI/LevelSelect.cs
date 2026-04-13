using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Settings menu
/// Should include sliders and toggles for player preferences
/// Such as audio settings or accessibility settings
/// </summary>
public class LevelSelect : MenuBase
{
    [FormerlySerializedAs("BackButton")] [SerializeField] private Button _backButton;

    private void OnEnable()
    {
        _backButton.Select();
    }

    public override GameMenus MenuType()
    {
        return GameMenus.LevelSelectMenu;
    }

    public void Close()
    {
        UIMgr.Instance.HideMenu(GameMenus.LevelSelectMenu);
    }
}

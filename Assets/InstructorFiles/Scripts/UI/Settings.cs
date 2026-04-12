using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Settings menu
/// </summary>
public class Settings : MenuBase
{
    [Header("UI References")]
    [FormerlySerializedAs("BackButton")][SerializeField] private Button _backButton;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;

    private GameMenus _previousMenu;

    public override GameMenus MenuType()
    {
        return GameMenus.SettingsMenu;
    }

    private void OnEnable()
    {
        _backButton.Select();

        _masterSlider.value = AudioMgr.Instance.GlobalVolume;
        _soundSlider.value = AudioMgr.Instance.SfxVolume;
        _musicSlider.value = AudioMgr.Instance.MusicVolume;
    }

    public void SetPreviousMenu(GameMenus menu)
    {
        _previousMenu = menu;
    }

    public void Close()
    {
        UIMgr.Instance.HideMenu(GameMenus.SettingsMenu);

        if (_previousMenu == GameMenus.PauseMenu)
        {
            UIMgr.Instance.ShowMenu(GameMenus.PauseMenu);
        }

        SaveUtil.Save();
    }

    public void SetMasterVolume(float volume)
    {
        AudioMgr.Instance.GlobalVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        AudioMgr.Instance.SfxVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        AudioMgr.Instance.MusicVolume = volume;
    }
}
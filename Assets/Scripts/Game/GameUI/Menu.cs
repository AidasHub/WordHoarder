using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Utility;
using static WordHoarder.Utility.SaveUtility;

public abstract class Menu : MonoBehaviour
{
    [Header("Option Components")]
    [SerializeField]
    protected Dropdown resolutionsDropdown;
    [SerializeField]
    protected Toggle fullScreenToggle;
    [SerializeField]
    protected Slider volumeSlider;
    [SerializeField]
    protected Toggle audioToggle;
    [SerializeField]
    protected List<Button> saveGameButtons;
    [SerializeField]
    protected ILocalizationHelper menuLocalizationHelper;
    protected SaveData[] savesData;

    public void InitializeSaveMenu()
    {
        savesData = SaveUtility.GetSavedGames();
        string[] slotInfo = new string[savesData.Length];
        for (int i = 0; i < saveGameButtons.Count; i++)
        {
            if (savesData[i] == null)
            {
                slotInfo[i] = null;
            }
            else
            {
                var gameCompletion = (float)savesData[i].CurrentProgress * 100 / savesData[i].TotalProgress;
                slotInfo[i] = string.Format("{0:0.0}%", gameCompletion);
                saveGameButtons[i].interactable = true;
            }
        }
        menuLocalizationHelper.UpdateLanguageForSaveGameSlots(slotInfo);
    }

    public void InitializeGraphicsMenu()
    {
        Resolution[] resolutions = SettingsUtility.GetResolutions();
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (SettingsUtility.GetFullScreenMode())
            {
                if (Screen.currentResolution.width == resolutions[i].width && Screen.currentResolution.height == resolutions[i].height)
                    currentResolutionIndex = i;
            }
            else
            {
                if (Screen.width == resolutions[i].width && Screen.height == resolutions[i].height)
                    currentResolutionIndex = i;
            }
        }
        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = currentResolutionIndex;
        resolutionsDropdown.RefreshShownValue();

        bool isFullScreen = SettingsUtility.GetFullScreenMode();
        fullScreenToggle.isOn = isFullScreen;
    }

    public void SetResolution()
    {
        string[] resolutionString = resolutionsDropdown.options[resolutionsDropdown.value].text.Split('x');
        int width = int.Parse(resolutionString[0]);
        int height = int.Parse(resolutionString[1]);
        SettingsUtility.SetResolution(width, height);
    }

    public void SetFullScreenMode(bool isFullScreen)
    {
        SettingsUtility.SetFullScreenMode(isFullScreen);
    }

    public void InitializeAudioMenu()
    {
        volumeSlider.value = SettingsUtility.GetAudioVolume();
        audioToggle.isOn = SettingsUtility.GetAudioEnabled();
    }

    public void SetAudioVolume(float volume)
    {
        SettingsUtility.SetAudioVolume(volume);
    }

    public void SetAudioEnabled(bool isEnabled)
    {
        SettingsUtility.SetAudioEnabled(isEnabled);
    }

    public void PlayAudioSample()
    {
        SoundManager.PlaySound(SoundManager.Sound.Test);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}

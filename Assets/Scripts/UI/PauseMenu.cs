using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject background;

    [SerializeField]
    private Dropdown resolutionsDropdown;
    [SerializeField]
    private Toggle fullScreenToggle;

    private int orderInHierarchy = 0;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeInHierarchy)
        {
            orderInHierarchy = transform.GetSiblingIndex();
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            pauseMenu.SetActive(true);
            background.SetActive(true);
            GameManager.GamePaused = true;
        }
    }

    public void ResumeGame()
    {
        transform.SetSiblingIndex(orderInHierarchy);
        pauseMenu.SetActive(false);
        background.SetActive(false);
        GameManager.GamePaused = false;
    }

    public void SaveGame()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void InitializeGraphicsMenu()
    {
        Resolution[] resolutions = SettingsManager.GetResolutions();
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (SettingsManager.GetFullScreenMode())
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
        resolutionsDropdown.value = currentResolutionIndex; // To-do: fix issue with resolution
        resolutionsDropdown.RefreshShownValue();

        bool isFullScreen = SettingsManager.GetFullScreenMode();
        fullScreenToggle.isOn = isFullScreen;
    }

    public void SetResolution()
    {
        string[] resolutionString = resolutionsDropdown.options[resolutionsDropdown.value].text.Split('x');
        int width = int.Parse(resolutionString[0]);
        int height = int.Parse(resolutionString[1]);
        SettingsManager.SetResolution(width, height);
    }

    public void SetFullScreenMode(bool isFullScreen)
    {
        SettingsManager.SetFullScreenMode(isFullScreen);
    }
}

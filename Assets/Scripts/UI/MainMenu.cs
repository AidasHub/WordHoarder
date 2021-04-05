using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator titleAnimator;
    [SerializeField]
    private TextMeshProUGUI titleTMP;
    [SerializeField]
    private GameObject buttonPanel;

    [Header("Option Components")]
    [SerializeField]
    private Dropdown resolutionsDropdown;
    [SerializeField]
    private Toggle fullScreenToggle;


    private string sourceTitleHalf = "WORD-";

    private void Awake()
    {
        Init();
    }


    public void Init()
    {
        StartCoroutine(DisplayTitle());
    }

    private IEnumerator DisplayTitle()
    {
        yield return new WaitForSeconds(1f);

        for(int i = 0; i < sourceTitleHalf.Length; i++)
        {
            titleTMP.text = titleTMP.text + sourceTitleHalf[i];
            yield return new WaitForSeconds(0.3f);
        }

        titleAnimator.SetTrigger("DisplayTitleSecondHalf");

    }

    public void EnableButtonInteraction()
    {
        var buttons = buttonPanel.GetComponentsInChildren<Button>();
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void StartGame()
    {
        titleAnimator.SetBool("DisplayLoadingScreen", true);
    }

    private void StartGameProcess()
    {
        GameManager.getInstance().InitGame();
        LevelManager.LoadTutorial();
    }

    public void LoadGame()
    {

    }

    public void InitializeGraphicsMenu()
    {
        Resolution[] resolutions = SettingsManager.GetResolutions();
        resolutionsDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if(SettingsManager.GetFullScreenMode())
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

    public void QuitGame()
    {
        Application.Quit();
    }
}

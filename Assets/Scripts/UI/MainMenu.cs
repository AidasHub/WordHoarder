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
    private AnimationClip loadingFadeClip;
    [SerializeField]
    private TextMeshProUGUI titleTMP;
    [SerializeField]
    private GameObject buttonPanel;

    [SerializeField]
    private List<Button> loadSlotButtons;


    [Header("Option Components")]
    [SerializeField]
    private Dropdown resolutionsDropdown;
    [SerializeField]
    private Toggle fullScreenToggle;


    private string sourceTitleHalf = "WORD-";
    private SaveData[] savesData;

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
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = loadingFadeClip.length - 0.01f;
        animEvent.functionName = "StartGameProcess";
        loadingFadeClip.AddEvent(animEvent);
        titleAnimator.SetBool("DisplayLoadingScreen", true);
    }

    private void StartGameProcess()
    {
        _SetupManager.getInstance().InitGame();
        LevelManager.LoadTutorial();
    }


    public void InitializeLoadMenu()
    {
        savesData = SaveManager.GetSavedGames();
        for(int i = 0; i < savesData.Length; i++)
        {
            if(savesData[i] != null)
            {
                loadSlotButtons[i].GetComponentInChildren<Text>().text = savesData[i].CollectedWords + "/" + savesData[i].TotalWords;
                loadSlotButtons[i].interactable = true;
            }
        }
    }

    public void LoadGame(int index)
    {
        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = loadingFadeClip.length - 0.01f;
        animEvent.intParameter = index;
        animEvent.functionName = "LoadGameProcess";
        loadingFadeClip.AddEvent(animEvent);
        titleAnimator.SetBool("DisplayLoadingScreen", true);
    }

    private void LoadGameProcess(int index)
    {
        _SetupManager.getInstance().InitGame();
        LevelManager.LoadExistingGame(savesData[index]);
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
        resolutionsDropdown.value = currentResolutionIndex;
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

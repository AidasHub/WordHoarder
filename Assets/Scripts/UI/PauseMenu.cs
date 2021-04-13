using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [Header("Main")]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject background;
    private bool menuOpen = false;

    [Header("Save Menu")]
    [SerializeField] 
    private Button saveOverwriteButtonYes;
    [SerializeField]
    private Text saveConfirmationLabel;
    [SerializeField]
    private List<Button> saveSlotButtons;
    private SaveData[] savesData;

    [Header("Graphics Settings")]
    [SerializeField]
    private Dropdown resolutionsDropdown;
    [SerializeField]
    private Toggle fullScreenToggle;

    private int orderInHierarchy = 0;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuOpen)
        {
            orderInHierarchy = transform.GetSiblingIndex();
            transform.SetSiblingIndex(transform.parent.childCount - 1);
            pauseMenu.SetActive(true);
            background.SetActive(true);
            GameManager.GamePaused = true;
            menuOpen = true;
        }
    }

    public void ResumeGame()
    {
        transform.SetSiblingIndex(orderInHierarchy);
        pauseMenu.SetActive(false);
        background.SetActive(false);
        GameManager.GamePaused = false;
        menuOpen = false;
    }

    public void InitializeSaveMenu()
    {
        savesData = SaveManager.GetSavedGames();
        for (int i = 0; i < saveSlotButtons.Count; i++)
        {
            if(savesData[i] == null)
            {
                saveSlotButtons[i].GetComponentInChildren<Text>().text = "EMPTY";
            }
            else
            {
                saveSlotButtons[i].GetComponentInChildren<Text>().text = savesData[i].CollectedWords + "/" + savesData[i].TotalWords;
            }
        }
    }

    public void SaveGameCheckOverwriting(int i)
    {
        if(savesData[i] == null)
        {
            SaveGame(i, false);
        }
        else
        {
            SaveGame(i, true);
        }
    }

    private void SaveGame(int i, bool overwriting)
    {
        if (overwriting)
        {
            saveOverwriteButtonYes.onClick.AddListener(() => SaveGame(i));
            saveOverwriteButtonYes.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            SaveGame(i);
        }
    }

    private void SaveGame(int i)
    {
        Debug.Log("Saving game...");
        GameScenario gameScenario = UIManager.getInstance().GetFromCanvas("GamePanel").GetComponent<GameScenario>();
        if (gameScenario != null)
        {
            bool success = SaveManager.SaveGame(gameScenario, i);
            SaveGameReportSuccess(success);
        }
        else
        {
            Debug.LogError("Saving game failed - no Game Scenario found.");
            SaveGameReportSuccess(false);
        }
    }

    private void SaveGameReportSuccess(bool success)
    {
        if(success)
        {
            saveConfirmationLabel.text = "SAVE SUCCESSFUL";
        }
        else
        {
            saveConfirmationLabel.text = "SAVING FAILED...";
        }
        saveConfirmationLabel.transform.parent.gameObject.SetActive(true);
    }

    public void ClearSaveListeners()
    {
        saveOverwriteButtonYes.onClick.RemoveAllListeners();
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WordHoarder.Gameplay.GameScenarios;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;
using WordHoarder.Utility;
using static WordHoarder.Utility.SaveUtility;

namespace WordHoarder.Gameplay.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField]
        private GameObject pauseMenu;
        [SerializeField]
        private GameObject background;
        [SerializeField]
        private PauseMenuLocalizationHelper pauseMenuLocalizationHelper;
        private bool menuOpen = false;

        [Header("Save Menu")]
        [SerializeField]
        private Button pauseSaveButton;
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

        [Header("Audio Settings")]
        [SerializeField]
        private Slider volumeSlider;
        [SerializeField]
        private Toggle audioToggle;

        private int orderInHierarchy = 0;

        public void Awake()
        {
            Button[] buttons = gameObject.GetComponentsInChildren<Button>(true);
            foreach (Button b in buttons)
                b.onClick.AddListener(() => SoundManager.PlaySound(SoundManager.Sound.Click));
        }

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
                var isTutorial = UIManager.GetFromCanvas("GamePanel") == null;
                if (isTutorial)
                    pauseSaveButton.interactable = false;
                else
                    pauseSaveButton.interactable = true;
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
            savesData = SaveUtility.GetSavedGames();
            string[] slotInfo = new string[savesData.Length];
            for (int i = 0; i < saveSlotButtons.Count; i++)
            {
                if (savesData[i] == null)
                {
                    slotInfo[i] = null;
                }
                else
                {
                    var gameCompletion = (float)savesData[i].CurrentProgress * 100 / savesData[i].TotalProgress;
                    slotInfo[i] = string.Format("{0:0.0}%", gameCompletion);
                }
            }
            pauseMenuLocalizationHelper.UpdateLanguageForSaveSlots(slotInfo);
        }

        public void SaveGameCheckOverwriting(int i)
        {
            if (savesData[i] == null)
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
            GameScenario gameScenario = UIManager.GetFromCanvas("GamePanel").GetComponent<GameScenario>();
            if (gameScenario != null)
            {
                bool success = SaveUtility.SaveGame(gameScenario.PrepareSaveData(), i);
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
            pauseMenuLocalizationHelper.UpdateLanguageForSaveSuccess(success);
            saveConfirmationLabel.transform.parent.gameObject.SetActive(true);
        }

        public void ClearSaveListeners()
        {
            saveOverwriteButtonYes.onClick.RemoveAllListeners();
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

        public void QuitToMainMenu()
        {
            GameManager.GamePaused = false;
            GameSetup.GetInstance().ReturnToMainMenu();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
            Application.Quit();
        }
    }
}
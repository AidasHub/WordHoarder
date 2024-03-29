using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Setup;
using WordHoarder.Utility;
using static WordHoarder.Utility.SaveUtility;

namespace WordHoarder.Gameplay.UI
{

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
        private MainMenuLocalizationHelper mainMenuLocalizationHelper;

        [SerializeField]
        private List<Button> loadSlotButtons;


        [Header("Option Components")]
        [SerializeField]
        private Dropdown resolutionsDropdown;
        [SerializeField]
        private Toggle fullScreenToggle;
        [SerializeField]
        private Slider audioSlider;
        [SerializeField]
        private Toggle audioEnabled;


        private string sourceTitleHalf = "WORD-";
        private SaveData[] savesData;

        private void Awake()
        {
            Init();
        }


        public void Init()
        {
            Button[] buttons = GameObject.FindObjectsOfType<Button>(true);
            foreach (Button b in buttons)
                b.onClick.AddListener(() => SoundManager.PlaySound(SoundManager.Sound.Click));
            StartCoroutine(DisplayTitle());
        }

        private IEnumerator DisplayTitle()
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < sourceTitleHalf.Length; i++)
            {
                titleTMP.text = titleTMP.text + sourceTitleHalf[i];
                yield return new WaitForSeconds(0.3f);
            }

            titleAnimator.SetTrigger("DisplayTitleSecondHalf");

        }

        public void EnableButtonInteraction()
        {
            var buttons = buttonPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
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
            GameSetup.GetInstance().InitGame();
            LevelManager.LoadTutorial();
        }


        public void InitializeLoadMenu()
        {
            savesData = SaveUtility.GetSavedGames();
            string[] slotInfo = new string[savesData.Length];
            for (int i = 0; i < savesData.Length; i++)
            {
                if (savesData[i] != null)
                {
                    var gameCompletion = (float)savesData[i].CurrentProgress * 100 / savesData[i].TotalProgress;
                    slotInfo[i] = string.Format("{0:0.0}%", gameCompletion);
                    loadSlotButtons[i].interactable = true;
                }
                else
                {
                    slotInfo[i] = null;
                }
            }
            mainMenuLocalizationHelper.UpdateLanguageForLoadSlots(slotInfo);
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
            GameSetup.GetInstance().InitGame();
            LevelManager.LoadExistingGame(savesData[index]);
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
            audioSlider.value = SettingsUtility.GetAudioVolume();
            audioEnabled.isOn = SettingsUtility.GetAudioEnabled();
        }

        public void PlayAudioSample()
        {
            SoundManager.PlaySound(SoundManager.Sound.Test);
        }

        public void SetAudioVolume(float volume)
        {
            SettingsUtility.SetAudioVolume(volume);
        }

        public void SetAudioEnabled(bool audioEnabled)
        {
            SettingsUtility.SetAudioEnabled(audioEnabled);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WordHoarder.Gameplay.GameScenarios;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;
using WordHoarder.Utility;

namespace WordHoarder.Gameplay.UI
{
    public class PauseMenu : Menu
    {
        [Header("Main")]
        [SerializeField]
        private GameObject pauseMenu;
        [SerializeField]
        private GameObject background;
        private bool menuOpen = false;

        [Header("Save Menu")]
        [SerializeField]
        private Button pauseSaveButton;
        [SerializeField]
        private Button saveOverwriteButtonYes;
        [SerializeField]
        private Text saveConfirmationLabel;

        private int orderInHierarchy = 0;

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            menuLocalizationHelper = GetComponentInChildren<PauseMenuLocalizationHelper>();
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

        public void QuitToMainMenu()
        {
            GameManager.GamePaused = false;
            GameSetup.GetInstance().ReturnToMainMenu();
        }

        private void SaveGameReportSuccess(bool success)
        {
            menuLocalizationHelper.UpdateLanguageForSaveSuccess(success);
            saveConfirmationLabel.transform.parent.gameObject.SetActive(true);
        }

        public void ClearSaveListeners()
        {
            saveOverwriteButtonYes.onClick.RemoveAllListeners();
        }
    }
}
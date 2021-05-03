using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Utility;

namespace WordHoarder.Gameplay.UI
{
    public class PauseMenuLocalizationHelper : MonoBehaviour
    {
        [Header("Main Menu Panel Text")]
        [SerializeField]
        Text pauseLabel;
        [SerializeField]
        Text pauseResume;
        [SerializeField]
        Text pauseSave;
        [SerializeField]
        Text pauseOptions;
        [SerializeField]
        Text pauseQuit;
        [SerializeField]
        Text pauseQuitConfirmation;
        [SerializeField]
        Text pauseQuitCancel;
        [SerializeField]
        Text pauseQuitMainMenu;
        [SerializeField]
        Text pauseQuitDesktop;

        [Header("Save Menu Panel Text")]
        [SerializeField]
        List<Text> saveSlots;
        [SerializeField]
        Text saveBack;
        [SerializeField]
        Text saveOverwriteLabel;
        [SerializeField]
        Text saveOverwriteYes;
        [SerializeField]
        Text saveOverwriteNo;
        [SerializeField]
        Text saveSuccessLabel;
        [SerializeField]
        Text saveSuccessButton;


        [Header("Options Menu Panel Text")]
        [SerializeField]
        Text optionsGraphics;
        [SerializeField]
        Text optionsAudio;
        [SerializeField]
        Text optionsBack;

        [Header("Graphics Menu Panel Text")]
        [SerializeField]
        Text graphicsResolution;
        [SerializeField]
        Text graphicsFullScreen;
        [SerializeField]
        Text graphicsBack;

        [Header("Audio Menu Panel Text")]
        [SerializeField]
        Text audioVolume;
        [SerializeField]
        Text audioEnabled;
        [SerializeField]
        Text audioBack;

        private void Start()
        {
            UpdateLanguage();
        }

        public void SetLanguage(int index)
        {
            LocalizationManager.SetActiveLanguage(index);
            UpdateLanguage();
        }

        public void UpdateLanguage()
        {
            var language = LocalizationManager.GetActiveLanguage();

            pauseLabel.text = language.PauseLabel.ToUpper();
            pauseResume.text = language.PauseResume.ToUpper();
            pauseSave.text = language.PauseSave.ToUpper();
            pauseOptions.text = language.MenuOptions.ToUpper();
            pauseQuit.text = language.PauseQuit.ToUpper();
            pauseQuitConfirmation.text = language.PauseQuitConfirmation.ToUpper();
            pauseQuitCancel.text = language.MiscCancel.ToUpper();
            pauseQuitMainMenu.text = language.PauseQuitMainMenu.ToUpper();
            pauseQuitDesktop.text = language.PauseQuitDesktop.ToUpper();

            saveBack.text = language.MiscBack.ToUpper();
            saveOverwriteLabel.text = language.LoadSlotOverwrite.ToUpper();
            saveOverwriteYes.text = language.MiscYes;
            saveOverwriteNo.text = language.MiscNo;
            saveSuccessButton.text = language.MiscOK;

            optionsGraphics.text = language.OptionsGraphics.ToUpper();
            optionsAudio.text = language.OptionsAudio.ToUpper();
            optionsBack.text = language.MiscBack.ToUpper();

            graphicsResolution.text = language.GraphicsResolution.ToUpper();
            graphicsFullScreen.text = language.GraphicsFullScreen.ToUpper();
            graphicsBack.text = language.MiscBack.ToUpper();

            audioVolume.text = language.AudioVolume.ToUpper();
            audioEnabled.text = language.AudioEnabled.ToUpper();
            audioBack.text = language.MiscBack.ToUpper();
        }

        public void UpdateLanguageForSaveSlots(string[] slotInfo)
        {
            var language = LocalizationManager.GetActiveLanguage();

            for (int i = 0; i < saveSlots.Count; i++)
            {
                if (slotInfo[i] == null)
                {
                    saveSlots[i].text = language.LoadSlotEmpty;
                }
                else
                {
                    saveSlots[i].text = language.WordsCollected.ToUpper() + ": " + slotInfo[i];
                }
            }
        }

        public void UpdateLanguageForSaveSuccess(bool isSuccessful)
        {
            var language = LocalizationManager.GetActiveLanguage();
            if (isSuccessful)
            {
                saveSuccessLabel.text = language.LoadSaveSuccessful;
            }
            else
            {
                saveSuccessLabel.text = language.LoadSaveFailed;
            }
        }
    }
}
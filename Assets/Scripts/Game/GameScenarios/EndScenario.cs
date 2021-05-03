using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;

namespace WordHoarder.Gameplay.GameScenarios
{
    public class EndScenario : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI Congratulations;
        [SerializeField]
        TextMeshProUGUI CompletionText;
        [SerializeField]
        TextMeshProUGUI Button;

        public void Awake()
        {
            LocalizationManager.onLanguageChanged += UpdateLanguage;
            UpdateLanguage();
        }

        public void ReturnToMainMenu()
        {
            GameSetup.GetInstance().ReturnToMainMenu();
        }

        private void UpdateLanguage()
        {
            var language = LocalizationManager.GetActiveLanguage();
            Congratulations.text = language.EndCongratulations;
            CompletionText.text = language.EndCompletion;
            Button.text = language.EndAwesome;
        }
    }
}
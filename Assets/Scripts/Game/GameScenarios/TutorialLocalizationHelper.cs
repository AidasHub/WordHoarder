using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using WordHoarder.Managers.Static.UI;

namespace WordHoarder.Gameplay.GameScenarios
{
    public class TutorialLocalizationHelper : MonoBehaviour
    {
        [SerializeField]
        TutorialScenario tutorialScenario;

        [Header("Tutorial Text")]
        [SerializeField]
        TextMeshProUGUI tutorialSteps;

        [Header("Navigation Text")]
        [SerializeField]
        TextMeshProUGUI buttonNext;
        [SerializeField]
        TextMeshProUGUI buttonLaunch;

        private void Start()
        {
            LocalizationManager.onLanguageChanged += UpdateLanguage;
            UpdateLanguage();
        }

        public void UpdateLanguage()
        {
            var language = LocalizationManager.GetActiveLanguage();

            buttonNext.text = language.MiscContinue;
            buttonLaunch.text = language.MiscLaunch;
        }
    }
}
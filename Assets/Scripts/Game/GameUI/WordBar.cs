using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;

namespace WordHoarder.Gameplay.UI
{
    public class WordBar : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TextMeshProUGUI sliderText;
        private float collectionPercentage;

        private static WordBar instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                Init();
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        public void Init()
        {
            GameManager.onProgressMade.AddListener(UpdateProgression);
            slider.maxValue = GameManager.TotalWords + GameManager.TotalEnvironments + GameManager.TotalHiddenObjects;
            UpdateProgression();
            LocalizationManager.onLanguageChanged += UpdateProgression;
        }

        public void UpdateProgression()
        {
            int currentCompletion = GameManager.CollectedWords + GameManager.UnlockedEnvironments + GameManager.RevealedHiddenObjects;
            int totalCompletion = GameManager.TotalWords + GameManager.TotalEnvironments + GameManager.TotalHiddenObjects;
            slider.value = currentCompletion;
            //sliderText.text = GameManager.CollectedWords + "/" + GameManager.TotalWords + " words collected";
            collectionPercentage = (100 * (float)currentCompletion/ totalCompletion);
            sliderText.text = string.Format("{0}: {1:0.0}%", LocalizationManager.GetActiveLanguage().WordsCollected, collectionPercentage);
        }

        public static void ShowWordBar()
        {
            instance.gameObject.SetActive(true);
        }

        public static void HideWordBar()
        {
            instance.gameObject.SetActive(false);
        }
    }
}
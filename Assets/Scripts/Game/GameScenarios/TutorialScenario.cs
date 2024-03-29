using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Gameplay.World;
using WordHoarder.Utility;

namespace WordHoarder.Gameplay.GameScenarios
{
    public class TutorialScenario : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI tutorialText;
        [SerializeField]
        private Button tutorialButtonNext;
        [SerializeField]
        private Button tutorialButtonStartGame;
        [SerializeField]
        private GameObject key;
        [SerializeField]
        private GameObject doorClosed;
        [SerializeField]
        private GameObject doorOpen;
        [SerializeField]
        private GameObject arrowImage;

        private WorldWord objectWorldWord;
        private string[] tutorialStepsText;
        private int currentTutorialStep;

        private void Awake()
        {
            objectWorldWord = key.GetComponentInChildren<WorldWord>();

            Init();
        }

        private void Update()
        {
            if(currentTutorialStep == 4)
            {
                if (InventoryManager.GetWords().Count > 0)
                    AdvanceTutorial();
            }
        }

        private void Init()
        {
            currentTutorialStep = 0;
            objectWorldWord.enabled = false;
            tutorialButtonStartGame.onClick.AddListener(AdvanceTutorial);
            LocalizationManager.onLanguageChanged += ForceStepsLanguageUpdate;
            ForceStepsLanguageUpdate();
            AdvanceTutorial();
        }

        public void ForceStepsLanguageUpdate()
        {
            tutorialStepsText = LocalizationManager.GetActiveLanguage().TutorialSteps;
            if (currentTutorialStep == 0)
                tutorialText.text = tutorialStepsText[0];
            else
                tutorialText.text = tutorialStepsText[currentTutorialStep - 1];
        }

        public void AdvanceTutorial()
        {
            if (currentTutorialStep < tutorialStepsText.Length)
                tutorialText.text = tutorialStepsText[currentTutorialStep];
            switch (currentTutorialStep)
            {
                case 2:
                    key.SetActive(true);
                    break;
                case 3:
                    tutorialButtonNext.gameObject.SetActive(false);
                    objectWorldWord.enabled = true;
                    break;
                case 4:
                    if (InventoryManager.GetWords().Count != 1)
                    {
                        tutorialText.text = tutorialStepsText[currentTutorialStep - 1];
                        return;
                    }
                    GameObject inventory = InventoryManager.GetInventoryGO();
                    inventory.transform.SetSiblingIndex(inventory.transform.parent.childCount - 1);
                    inventory.SetActive(true);
                    inventory.GetComponentInChildren<Button>().onClick.AddListener(AdvanceTutorial);
                    arrowImage.SetActive(true);
                    break;
                case 5:
                    InventoryManager.GetInventoryGO().GetComponentInChildren<Button>().onClick.RemoveListener(AdvanceTutorial);
                    key.SetActive(false);
                    arrowImage.SetActive(false);
                    doorClosed.SetActive(true);
                    break;
                case 6:
                    doorOpen.SetActive(true);
                    doorClosed.SetActive(false);
                    tutorialButtonStartGame.gameObject.SetActive(true);
                    break;
                case 7:
                    LocalizationManager.onLanguageChanged -= ForceStepsLanguageUpdate;
                    LevelManager.LoadNewGame();
                    Destroy(this.gameObject);
                    break;
                default:
                    break;
            }
            currentTutorialStep++;
        }
    }
}
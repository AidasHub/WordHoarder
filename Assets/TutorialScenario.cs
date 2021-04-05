using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialScenario : MonoBehaviour
{

    private UIManager _UIManager;

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
    private Button keyButton;
    private string[] tutorialStepsText;
    private int currentTutorialStep;

    private void Awake()
    {
        _UIManager = UIManager.getInstance();
        objectWorldWord = key.GetComponent<WorldWord>();
        keyButton = key.GetComponent<Button>();

        Init();
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
                keyButton.enabled = false;
                break;
            case 3:
                tutorialButtonNext.gameObject.SetActive(false);
                objectWorldWord.enabled = true;
                keyButton.enabled = true;
                break;
            case 4:
                GameObject inventory = _UIManager.getFromCanvas("Inventory");
                inventory.transform.SetSiblingIndex(inventory.transform.parent.childCount - 1);
                inventory.SetActive(true);
                inventory.GetComponentInChildren<Button>().onClick.AddListener(AdvanceTutorial);
                arrowImage.SetActive(true);
                break;
            case 5:
                _UIManager.getFromCanvas("Inventory").GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                key.SetActive(false);
                arrowImage.SetActive(false);
                doorClosed.SetActive(true);
                break;
            case 6:
                InventoryManager.getInstance().ToggleInventory();
                doorOpen.SetActive(true);
                doorClosed.SetActive(false);
                tutorialButtonStartGame.gameObject.SetActive(true);
                break;
            case 7:
                LevelManager.LoadNewGame();
                Destroy(this.gameObject);
                break;
            default:
                break;
        }
        currentTutorialStep++;
    }
}

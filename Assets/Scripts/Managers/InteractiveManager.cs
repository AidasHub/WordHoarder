using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class InteractiveManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private GameObject interactivePanelPrefab;

    private GameObject interactivePanel;
    private Button interactivePanelCloseButton;
    private Animator animator;

    [Header("Interaction Scripts")]
    [SerializeField]
    private PuzzleWordFill puzzleWordFill;
    [SerializeField]
    private List<TextAsset> wordFillPuzzles;


    private static InteractiveManager instance;

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

    private void Init()
    {
        interactivePanel = UIManager.getInstance().AddToCanvas(interactivePanelPrefab);
        interactivePanelCloseButton = interactivePanel.GetComponentInChildren<Button>();
        animator = interactivePanel.GetComponent<Animator>();
        puzzleWordFill = interactivePanel.GetComponentInChildren<PuzzleWordFill>();
    }

    public static InteractiveManager getInstance()
    {
        return instance;
    }

    public GameObject GetInteractivePanelGO()
    {
        return interactivePanel;
    }

    public void ToggleInteraction()
    {
        if(!animator.GetBool("InteractionPanelEnabled"))
        {
            InventoryManager.getInstance().DisableInventory();
            WordBar.HideWordBar();
            animator.SetBool("InteractionPanelEnabled", true);
        }
        else
        {
            InventoryManager.getInstance().EnableInventory();
            WordBar.ShowWordBar();
            animator.SetBool("InteractionPanelEnabled", false);
        }
    }

    public void LoadWordFillPuzzle(int index, UnityAction rewardAction)
    {
        puzzleWordFill.InitPuzzle(wordFillPuzzles[index], rewardAction);
        ToggleInteraction();
        interactivePanelCloseButton.onClick.RemoveAllListeners();
        interactivePanelCloseButton.onClick.AddListener(puzzleWordFill.Close);
        interactivePanelCloseButton.onClick.AddListener(ToggleInteraction);
    }
}

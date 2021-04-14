using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public static class InteractiveManager
{
    private static GameObject interactivePanelPrefab;

    private static GameObject interactivePanel;
    private static Button interactivePanelCloseButton;
    private static Animator animator;

    private static PuzzleWordFill puzzleWordFill;
    //
    private static List<TextAsset> wordFillPuzzles;

    public static void Init()
    {
        if(interactivePanel == null)
        {
            interactivePanelPrefab = AssetsManager.ManagerPrefabs.InteractivePanel;
            wordFillPuzzles = AssetsManager.Puzzles.WordFillPuzzles;
            Debug.Log(wordFillPuzzles.Count);

            interactivePanel = UIManager.AddToCanvas(interactivePanelPrefab);
            interactivePanelCloseButton = interactivePanel.GetComponentInChildren<Button>();
            animator = interactivePanel.GetComponent<Animator>();
            puzzleWordFill = interactivePanel.GetComponentInChildren<PuzzleWordFill>();
        }
    }

    public static GameObject GetInteractivePanelGO()
    {
        if(interactivePanel == null)
        {
            Init();
        }
        return interactivePanel;
    }

    public static void ToggleInteraction()
    {
        if(interactivePanel == null)
        {
            Init();
        }
        if(!animator.GetBool("InteractionPanelEnabled"))
        {
            InventoryManager.DisableInventory();
            WordBar.HideWordBar();
            animator.SetBool("InteractionPanelEnabled", true);
        }
        else
        {
            InventoryManager.EnableInventory();
            WordBar.ShowWordBar();
            animator.SetBool("InteractionPanelEnabled", false);
        }
    }

    public static void LoadWordFillPuzzle(int index, UnityAction rewardAction)
    {
        if(interactivePanel == null)
        {
            Init();
        }
        puzzleWordFill.InitPuzzle(wordFillPuzzles[index], rewardAction);
        ToggleInteraction();
        interactivePanelCloseButton.onClick.RemoveAllListeners();
        interactivePanelCloseButton.onClick.AddListener(puzzleWordFill.Close);
        interactivePanelCloseButton.onClick.AddListener(ToggleInteraction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using WordHoarder.Gameplay.Puzzles;
using WordHoarder.UI;
using WordHoarder.Managers.Instanced;

namespace WordHoarder.Managers.Static.UI
{
    public static class InteractiveManager
    {
        private static GameObject interactivePanelPrefab;

        private static GameObject interactivePanel;
        private static Button interactivePanelCloseButton;
        private static Animator animator;
        private static GameObject activePuzzle;

        private static PuzzleWordFill puzzleWordFill;
        private static List<TextAsset> wordFillPuzzles;

        private static PuzzleRotatingLock puzzleRotatingLock;
        private static List<TextAsset> rotatingLockPuzzles;

        private static PuzzleImageGuess puzzleImageGuess;
        private static List<TextAsset> imageGuessPuzzles;

        private static bool interactivePanelOpen;
        public static bool InteractivePanelOpen
        {
            get
            {
                return interactivePanelOpen;
            }
            private set
            {
                interactivePanelOpen = value;
            }
        }

        public static void Init()
        {
            if (interactivePanel == null)
            {
                interactivePanelPrefab = AssetsManager.ManagerPrefabs.InteractivePanel;
                wordFillPuzzles = AssetsManager.Puzzles.WordFillPuzzles;
                rotatingLockPuzzles = AssetsManager.Puzzles.RotatingLockPuzzles;
                imageGuessPuzzles = AssetsManager.Puzzles.ImageGuessPuzzles;
                Debug.Log(wordFillPuzzles.Count);

                interactivePanel = UIManager.AddToCanvas(interactivePanelPrefab);
                interactivePanelCloseButton = interactivePanel.GetComponentInChildren<Button>();
                animator = interactivePanel.GetComponent<Animator>();
                puzzleWordFill = interactivePanel.GetComponentInChildren<PuzzleWordFill>(true);
                puzzleRotatingLock = interactivePanel.GetComponentInChildren<PuzzleRotatingLock>(true);
                puzzleImageGuess = interactivePanel.GetComponentInChildren<PuzzleImageGuess>(true);
            }
        }

        public static GameObject GetInteractivePanelGO()
        {
            if (interactivePanel == null)
            {
                Init();
            }
            return interactivePanel;
        }

        public static void ToggleInteraction()
        {
            if (interactivePanel == null)
            {
                Init();
            }
            if (!animator.GetBool("InteractionPanelEnabled"))
            {
                InventoryManager.DisableInventory();
                WordBar.HideWordBar();
                animator.SetBool("InteractionPanelEnabled", true);
                InteractivePanelOpen = true;
                TooltipManager.HideTooltip();
            }
            else
            {
                InventoryManager.EnableInventory();
                WordBar.ShowWordBar();
                animator.SetBool("InteractionPanelEnabled", false);
                InteractivePanelOpen = false;
            }
        }

        public static void LoadWordFillPuzzle(int index, UnityAction rewardAction)
        {
            if (interactivePanel == null)
            {
                Init();
            }
            if (activePuzzle != null)
                activePuzzle.SetActive(false);
            activePuzzle = puzzleWordFill.gameObject;
            activePuzzle.SetActive(true);
            puzzleWordFill.InitPuzzle(wordFillPuzzles[index], rewardAction);
            ToggleInteraction();
            interactivePanelCloseButton.onClick.RemoveAllListeners();
            interactivePanelCloseButton.onClick.AddListener(puzzleWordFill.Close);
            interactivePanelCloseButton.onClick.AddListener(ToggleInteraction);
        }

        public static void LoadRotatingLockPuzzle(int index, UnityAction rewardAction)
        {
            if (interactivePanel == null)
            {
                Init();
            }
            if (activePuzzle != null)
                activePuzzle.SetActive(false);
            activePuzzle = puzzleRotatingLock.gameObject;
            activePuzzle.SetActive(true);
            puzzleRotatingLock.InitPuzzle(rotatingLockPuzzles[index], rewardAction);
            ToggleInteraction();
            interactivePanelCloseButton.onClick.RemoveAllListeners();
            interactivePanelCloseButton.onClick.AddListener(puzzleRotatingLock.Close);
            interactivePanelCloseButton.onClick.AddListener(ToggleInteraction);
        }

        public static void LoadImageGuessPuzzle(int index, UnityAction rewardAction)
        {
            if (interactivePanel == null)
            {
                Init();
            }
            if (activePuzzle != null)
                activePuzzle.SetActive(false);
            activePuzzle = puzzleImageGuess.gameObject;
            activePuzzle.SetActive(true);
            puzzleImageGuess.InitPuzzle(imageGuessPuzzles[index], rewardAction);
            ToggleInteraction();
            interactivePanelCloseButton.onClick.RemoveAllListeners();
            interactivePanelCloseButton.onClick.AddListener(ToggleInteraction);
        }
    }
}
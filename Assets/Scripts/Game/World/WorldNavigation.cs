using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WordHoarder.Gameplay.GameScenarios;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;

namespace WordHoarder.Gameplay.World
{
    public class WorldNavigation : MonoBehaviour, IWorldSaveable
    {
        private enum EnvironmentDestination
        {
            ToCorridor,
            ToKitchen,
            ToKitchen2,
            ToLivingRoom,
            ToLivingRoom2,
            ToLivingRoom3,
            ToBathroom,
            ToBedroom
        }

        private enum PuzzleType
        {
            WordFill,
            RotatingLock,
            ImageGuess
        }

        [SerializeField]
        private Button lockedButton;
        [SerializeField]
        private Button unlockedButton;
        [SerializeField]
        private bool isLocked;
        [SerializeField]
        EnvironmentDestination destination;
        [SerializeField]
        PuzzleType puzzleType;
        [SerializeField]
        private int puzzleIndex;

        public void Awake()
        {
            SetupEnvironment();
        }

        public void SetupEnvironment()
        {
            if (isLocked)
            {
                lockedButton.gameObject.SetActive(true);
                unlockedButton.interactable = false;
            }
            else
            {
                if (lockedButton != null)
                    lockedButton.gameObject.SetActive(false);
                unlockedButton.gameObject.SetActive(true);
                unlockedButton.interactable = true;
            }

            GameScenario gameScenario = transform.parent.parent.parent.GetComponent<GameScenario>();
            if (gameScenario == null)
                Debug.LogError("Game Scenario not found");
            if (puzzleType == PuzzleType.WordFill)
                lockedButton.onClick.AddListener(() => PuzzleManager.LoadWordFillPuzzle(puzzleIndex, UnlockEnvironment));
            else if (puzzleType == PuzzleType.RotatingLock)
                lockedButton.onClick.AddListener(() => PuzzleManager.LoadRotatingLockPuzzle(puzzleIndex, UnlockEnvironment));
            else if (puzzleType == PuzzleType.ImageGuess)
                lockedButton.onClick.AddListener(() => PuzzleManager.LoadImageGuessPuzzle(puzzleIndex, UnlockEnvironment));

            unlockedButton.onClick.AddListener(() => gameScenario.SwitchEnvironment((int)destination));
        }

        public void UnlockEnvironment()
        {
            if (lockedButton != null)
                lockedButton.gameObject.SetActive(false);
            unlockedButton.interactable = true;
            isLocked = false;
            GameManager.IncreaseUnlockedEnvironments();
        }


        public Tuple<string, bool> PrepareSaveData()
        {
            string navigationTo = gameObject.name;
            Tuple<string, bool> saveData = new Tuple<string, bool>(navigationTo, !isLocked);
            return saveData;
        }

        public void LoadSaveData(bool isComplete)
        {
            if (isComplete)
                UnlockEnvironment();
        }

        private void OnMouseOver()
        {
            if (!PuzzleManager.InteractivePanelOpen && !GameManager.GamePaused)
            {
                string text;
                if (lockedButton.gameObject.activeInHierarchy)
                {
                    text = LocalizationManager.GetActiveLanguage().PuzzleTooltip;
                }
                else
                {
                    text = GetEnvironmentDestinationText(destination);
                }
                TooltipManager.DrawTooltip(text);
            }
        }

        private void OnMouseExit()
        {
            TooltipManager.HideTooltip();
        }

        private string GetEnvironmentDestinationText(EnvironmentDestination destination)
        {
            var language = LocalizationManager.GetActiveLanguage();
            switch (destination)
            {
                default:
                    return "Error";
                case EnvironmentDestination.ToCorridor:
                    return language.EnvironmentToCorridor;
                case EnvironmentDestination.ToKitchen:
                    return language.EnvironmentToKitchen;
                case EnvironmentDestination.ToKitchen2:
                    return language.EnvironmentToKitchen2;
                case EnvironmentDestination.ToLivingRoom:
                    return language.EnvironmentToLivingRoom;
                case EnvironmentDestination.ToLivingRoom2:
                    return language.EnvironmentToLivingRoom2;
                case EnvironmentDestination.ToLivingRoom3:
                    return language.EnvironmentToLivingRoom3;
                case EnvironmentDestination.ToBathroom:
                    return language.EnvironmentToBathroom;
                case EnvironmentDestination.ToBedroom:
                    return language.EnvironmentToBedroom;
            }
        }
    }
}

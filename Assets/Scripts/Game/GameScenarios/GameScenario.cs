using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WordHoarder.Gameplay.UI;
using WordHoarder.Gameplay.World;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Utility;
using static WordHoarder.Utility.SaveManager;

namespace WordHoarder.Gameplay.GameScenarios
{
    public class GameScenario : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> environments;
        public int CurrentEnvironment { get; private set; }

        public void Awake()
        {
            GameManager.TotalWords = GetComponentsInChildren<WorldWord>(true).Length;
        }

        public void SwitchEnvironment(int index)
        {
            environments[CurrentEnvironment].SetActive(false);
            environments[index].SetActive(true);
            CurrentEnvironment = index;
            TooltipManager.HideTooltip();
        }

        public SaveData PrepareSaveData()
        {
            int saveCurrentEnvironment = CurrentEnvironment;

            List<Tuple<string, bool>> saveEnvironmentStatus = new List<Tuple<string, bool>>();
            WorldNavigation[] environmentStatus = GetComponentsInChildren<WorldNavigation>(true);
            for (int i = 0; i < environmentStatus.Length; i++)
            {
                saveEnvironmentStatus.Add(environmentStatus[i].PrepareSaveData());
            }

            List<Tuple<string, bool>> saveWorldWords = new List<Tuple<string, bool>>();
            WorldWord[] worldWords = GetComponentsInChildren<WorldWord>(true);
            for (int i = 0; i < worldWords.Length; i++)
            {
                saveWorldWords.Add(worldWords[i].PrepareSaveData());
            }

            List<Tuple<string, bool>> saveReverseWords = new List<Tuple<string, bool>>();
            WorldInteractable[] reverseWords = GetComponentsInChildren<WorldInteractable>(true);
            for (int i = 0; i < reverseWords.Length; i++)
            {
                saveReverseWords.Add(reverseWords[i].PrepareSaveData());
            }

            List<string> saveInventoryWords = new List<string>();
            List<InventoryWord> inventoryWords = InventoryManager.GetWords();
            for (int i = 0; i < inventoryWords.Count; i++)
            {
                saveInventoryWords.Add(inventoryWords[i].GetWordString());
            }

            int saveCollectedWords = GameManager.CollectedWords;
            int totalWords = GameManager.TotalWords;

            SaveData saveData = new SaveData(saveCurrentEnvironment, saveEnvironmentStatus, saveWorldWords, saveReverseWords, saveInventoryWords, saveCollectedWords, totalWords);
            return saveData;
        }

        public void LoadSaveData(int lastEnvironment)
        {
            SwitchEnvironment(lastEnvironment);
        }
    }
}
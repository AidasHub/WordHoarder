using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WordHoarder.Gameplay.UI;
using WordHoarder.Gameplay.World;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;
using static WordHoarder.Utility.SaveUtility;

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
            GameManager.ClearCollectedWords();
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

        public void LoadSaveData(SaveData data)
        {
            SwitchEnvironment(data.CurrentEnvironment);

            // Interface based search is not an option since it does not convert into UnityEngine.Object
            WorldNavigation[] environmentStatus = GetComponentsInChildren<WorldNavigation>(true);
            WorldWord[] worldWords = GetComponentsInChildren<WorldWord>(true);
            WorldInteractable[] reverseWords = GetComponentsInChildren<WorldInteractable>(true);

            if (environmentStatus.Length != data.EnvironmentStatus.Count)
            {
                Debug.LogError("Error loading a save file - environment navigation count does not match");
                return;
            }
            if (worldWords.Length != data.WorldWords.Count)
            {
                Debug.LogError("Error loading a save file - world words count does not match");
                return;
            }
            if (reverseWords.Length != data.ReverseWords.Count)
            {
                Debug.LogError("Error loading a save file - world interactable count does not match");
                return;
            }

            for (int i = 0; i < environmentStatus.Length; i++)
            {
                for (int j = 0; j < data.EnvironmentStatus.Count; j++)
                {
                    if (environmentStatus[i].gameObject.name == data.EnvironmentStatus[j].Item1)
                    {
                        environmentStatus[i].LoadSaveData(data.EnvironmentStatus[j].Item2);
                        break;
                    }
                }
            }

            for (int i = 0; i < worldWords.Length; i++)
            {
                for (int j = 0; j < data.WorldWords.Count; j++)
                {
                    if (worldWords[i].gameObject.name == data.WorldWords[j].Item1)
                    {
                        worldWords[i].LoadSaveData(data.WorldWords[j].Item2);
                        break;
                    }
                }
            }

            for (int i = 0; i < reverseWords.Length; i++)
            {
                for (int j = 0; j < data.ReverseWords.Count; j++)
                {
                    if (reverseWords[i].gameObject.name == data.ReverseWords[j].Item1)
                    {
                        reverseWords[i].LoadSaveData(data.ReverseWords[j].Item2);
                        break;
                    }
                }
            }

            for (int i = 0; i < data.InventoryWords.Count; i++)
            {
                InventoryManager.AddWord(data.InventoryWords[i]);
            }

            GameManager.ClearCollectedWords();
            for (int i = 0; i < data.CollectedWords; i++)
            {
                GameManager.IncreaseCollectedWords();
            }

            Debug.Log("Data successfully loaded!");
        }
    }
}
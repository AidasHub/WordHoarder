using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using WordHoarder.GameScenarios;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Gameplay.World;
using WordHoarder.UI;

namespace WordHoarder.Managers.Static.Generic
{
    public static class SaveManager
    {
        [Serializable]
        public class SaveData
        {
            public int CurrentEnvironment;
            public List<Tuple<string, bool>> EnvironmentStatus;
            public List<Tuple<string, bool>> WorldWords;
            public List<string> InventoryWords;
            public int CollectedWords;
            public int TotalWords;

            public SaveData(int ce, List<Tuple<string, bool>> es, List<Tuple<string, bool>> ww, List<string> iw, int cw, int tw)
            {
                CurrentEnvironment = ce;
                EnvironmentStatus = es;
                WorldWords = ww;
                InventoryWords = iw;
                CollectedWords = cw;
                TotalWords = tw;
            }
        }

        private static int maxSavedGamesCount = 3;
        private static string saveFolderName = "SaveData";
        private static string saveFileName = "Data";


        public static bool SaveGame(GameScenario gameScenario, int saveSlot)
        {
            if (saveSlot >= maxSavedGamesCount)
                return false;
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + saveFolderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "/" + saveFileName + saveSlot;

            FileStream fileStream = new FileStream(path, FileMode.Create);
            bool success = false;
            try
            {
                GameObject gameScenarioGO = gameScenario.gameObject;
                int saveCurrentEnvironment = gameScenarioGO.GetComponent<GameScenario>().CurrentEnvironment;


                List<Tuple<string, bool>> saveEnvironmentStatus = new List<Tuple<string, bool>>();
                EnvironmentNavigation[] environmentStatus = gameScenarioGO.GetComponentsInChildren<EnvironmentNavigation>(true);
                for (int i = 0; i < environmentStatus.Length; i++)
                {
                    saveEnvironmentStatus.Add(environmentStatus[i].PrepareSaveData());
                }

                List<Tuple<string, bool>> saveWorldWords = new List<Tuple<string, bool>>();
                WorldWord[] worldWords = gameScenarioGO.GetComponentsInChildren<WorldWord>(true);
                for (int i = 0; i < worldWords.Length; i++)
                {
                    saveWorldWords.Add(worldWords[i].PrepareSaveData());
                }

                List<string> saveInventoryWords = new List<string>();
                List<InventoryWord> inventoryWords = InventoryManager.GetWords();
                for (int i = 0; i < inventoryWords.Count; i++)
                {
                    saveInventoryWords.Add(inventoryWords[i].getWordString());
                }

                int saveCollectedWords = GameManager.CollectedWords;
                int totalWords = GameManager.TotalWords;

                SaveData saveData = new SaveData(saveCurrentEnvironment, saveEnvironmentStatus, saveWorldWords, saveInventoryWords, saveCollectedWords, totalWords);

                formatter.Serialize(fileStream, saveData);
                Debug.Log("Game Data saved to " + path);
                success = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
            }
            finally
            {
                fileStream.Close();
            }
            return success;
        }

        public static SaveData LoadGame(int saveSlot)
        {
            string path = Application.persistentDataPath + "/" + saveFolderName;
            if (Directory.Exists(path))
            {
                path += "/" + saveFileName + saveSlot;
                if (File.Exists(path))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream fileStream = new FileStream(path, FileMode.Open);
                    try
                    {
                        SaveData data = formatter.Deserialize(fileStream) as SaveData;
                        return data;
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                        Debug.LogError(e.StackTrace);
                    }
                    finally
                    {
                        fileStream.Close();
                    }
                }
                else
                {
                    Debug.LogError("Save file was not found");
                }
            }
            else
            {
                Debug.LogError("Save Folder not found");
            }
            return null;
        }

        public static SaveData[] GetSavedGames()
        {
            SaveData[] savesData = new SaveData[maxSavedGamesCount];
            string path = Application.persistentDataPath + "/" + saveFolderName;
            if (!Directory.Exists(path))
                return savesData;

            for (int i = 0; i < maxSavedGamesCount; i++)
            {
                string filePath = path + "/" + saveFileName + i;
                if (File.Exists(filePath))
                {
                    savesData[i] = (LoadGame(i));
                }
            }
            return savesData;
        }
    }
}

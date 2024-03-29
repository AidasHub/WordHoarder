using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Gameplay.World;
using WordHoarder.Gameplay.UI;
using WordHoarder.Gameplay.GameScenarios;

namespace WordHoarder.Utility
{
    public static class SaveUtility
    {
        [Serializable]
        public class SaveData
        {
            public int CurrentEnvironment;
            public List<Tuple<string, bool>> EnvironmentStatus;
            public List<Tuple<string, bool>> WorldWords;
            public List<Tuple<string, bool>> ReverseWords;
            public List<string> InventoryWords;
            public int CollectedWords;
            public int CurrentProgress;
            public int TotalProgress;

            public SaveData(int ce, List<Tuple<string, bool>> es, List<Tuple<string, bool>> ww, 
                List<Tuple<string, bool>> rw, List<string> iw, int cw, int cp, int tp)
            {
                CurrentEnvironment = ce;
                EnvironmentStatus = es;
                WorldWords = ww;
                ReverseWords = rw;
                InventoryWords = iw;
                CollectedWords = cw;
                CurrentProgress = cp;
                TotalProgress = tp;
            }
        }

        private static int maxSavedGamesCount = 3;
        public static string SaveFolderName { get; private set; } = "SaveData";
        public static string SaveFileName { get; private set; } = "Data";


        public static bool SaveGame(SaveData saveData, int saveSlot)
        {
            if (saveSlot >= maxSavedGamesCount)
                return false;
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/" + SaveFolderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "/" + SaveFileName + saveSlot;

            FileStream fileStream = new FileStream(path, FileMode.Create);
            bool success = false;
            try
            {
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
            string path = Application.persistentDataPath + "/" + SaveFolderName;
            if (Directory.Exists(path))
            {
                path += "/" + SaveFileName + saveSlot;
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
            string path = Application.persistentDataPath + "/" + SaveFolderName;
            if (!Directory.Exists(path))
                return savesData;

            for (int i = 0; i < maxSavedGamesCount; i++)
            {
                string filePath = path + "/" + SaveFileName + i;
                if (File.Exists(filePath))
                {
                    savesData[i] = (LoadGame(i));
                }
            }
            return savesData;
        }
    }
}

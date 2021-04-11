using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public static class SaveManager
{
    public static void SaveGame(GameScenario gameScenario)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData";
        if(!Directory.Exists(path + "/SaveData"))
        {
            Directory.CreateDirectory(path);
        }
        path += "/Data";

        FileStream fileStream = new FileStream(path, FileMode.Create);
        try
        {
            GameObject gameScenarioGO = gameScenario.gameObject;
            int currentEnvironment = 0;
            for(int i = 0; i < gameScenarioGO.transform.childCount; i++)
            {
                if(gameScenarioGO.transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    currentEnvironment = i;
                    break;
                }
            }

            List<Tuple<string, bool>> saveEnvironmentStatus = new List<Tuple<string, bool>>();
            EnvironmentNavigation[] environmentStatus = gameScenarioGO.GetComponentsInChildren<EnvironmentNavigation>(true);
            for(int i = 0; i < environmentStatus.Length; i++)
            {
                saveEnvironmentStatus.Add(environmentStatus[i].PrepareSaveData());
            }

            List<Tuple<string, bool>> saveWorldWords = new List<Tuple<string, bool>>();
            WorldWord[] worldWords = gameScenarioGO.GetComponentsInChildren<WorldWord>(true);
            for(int i = 0; i < worldWords.Length; i++)
            {
                saveWorldWords.Add(worldWords[i].PrepareSaveData());
            }

            List<string> saveInventoryWords = new List<string>();
            List<InventoryWord> inventoryWords = InventoryManager.getInstance().GetWords();
            for(int i = 0; i < inventoryWords.Count; i++)
            {
                saveInventoryWords.Add(inventoryWords[i].getWordString());
            }

            SaveData saveData = new SaveData(currentEnvironment, saveEnvironmentStatus, saveWorldWords, saveInventoryWords);

            formatter.Serialize(fileStream, saveData);
            Debug.Log("Game Data saved to " + path);
        }
        catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            fileStream.Close();
        }

    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/SaveData";
        if (Directory.Exists(path))
        {
            path += "/Data";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(path, FileMode.Open);
                try
                {
                    SaveData data = formatter.Deserialize(fileStream) as SaveData;
                    return data;
                }
                catch(Exception e)
                {
                    Debug.LogError(e.Message);
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
}

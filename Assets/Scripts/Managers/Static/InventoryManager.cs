using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public static class InventoryManager
{
    private static GameObject inventoryPanelPrefab;

    public static bool IsOpen { get; private set; } = false;

    private static GameObject wordPrefab;

    private static GameObject inventory;
    private static Animator inventoryAnimator;
    private static Button inventoryButton;
    private static GridLayoutGroup wordGridLayout;

    static List<InventoryWord> wordList;

    public static void Init()
    {
        if(inventory == null)
        {
            inventoryPanelPrefab = AssetsManager.ManagerPrefabs.Inventory;
            wordPrefab = AssetsManager.Words.InventoryWord;
            inventory = UIManager.AddToCanvas(inventoryPanelPrefab);
            inventoryAnimator = inventory.GetComponent<Animator>();
            inventoryButton = inventory.GetComponentInChildren<Button>();
            inventoryButton.onClick.AddListener(ToggleInventory);
            wordList = new List<InventoryWord>();
        }
        wordGridLayout = inventory.GetComponentInChildren<GridLayoutGroup>();
    }

    public static void AddWord(string word)
    {
        if(inventory == null)
        {
            Init();
        }
        GameObject newWordGO = GameObject.Instantiate(wordPrefab, wordGridLayout.transform);
        newWordGO.name = word;
        InventoryWord newWord = newWordGO.GetComponent<InventoryWord>();
        TextMeshProUGUI newWordTMP = newWordGO.GetComponent<TextMeshProUGUI>();
        newWordTMP.text = word;
        wordList.Add(newWord);
        GameManager.IncreaseCollectedWords();
    }

    public static void RemoveWord(string word)
    {
        if(inventory == null)
        {
            Init();
        }
        for(int i = 0; i < wordList.Count; i++)
        {
            if (wordList[i].getWordString().ToLower() == word.ToLower())
            {
                wordList.RemoveAt(i);
                RefreshInventory();
                break;
            }          
        }
    }

    public static void RefreshInventory()
    {
        for(int i = 0; i < wordGridLayout.transform.childCount; i++)
        {
            bool found = false;
            for (int j = 0; j < wordList.Count; j++)
            {
                if (wordGridLayout.transform.GetChild(i).name == wordList[j].getWordString())
                    found = true;
            }
            if (!found)
            {
                GameObject.Destroy(wordGridLayout.transform.GetChild(i).gameObject);
            }
        }
    }

    public static List<InventoryWord> GetWords()
    {
        return wordList;
    }

    public static void ToggleInventory()
    {
        if (IsOpen)
        {
            IsOpen = false;
            inventoryAnimator.SetBool("InventoryOpen", false);
        }
        else
        {
            Debug.Log("Opening");
            IsOpen = true;
            inventoryAnimator.SetBool("InventoryOpen", true);
        }
    }

    public static GameObject GetInventoryGO()
    {
        return inventory;
    }

    public static void EnableInventory()
    {
        inventory.SetActive(true);
    }

    public static void DisableInventory()
    {
        inventory.SetActive(false);
    }

}

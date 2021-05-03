using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Gameplay.UI;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;

public class InventoryManagerTests
{
    public bool setupOnce = true;
    public bool setupComplete = false;

    [UnitySetUp]
    public IEnumerator TestSetup()
    {
        if (!setupComplete)
        {
            var testSceneOperation = SceneManager.LoadSceneAsync("TestScene");
            while (!testSceneOperation.isDone)
                yield return null;
        }

        if (GameSetup.GetInstance() != null)
        {
            GameObject.Destroy(GameSetup.GetInstance().transform.parent.gameObject);
        }

        var managers = new GameObject("Managers");
        var setupManager = new GameObject("Setup");
        setupManager.transform.SetParent(managers.transform);
        setupManager.AddComponent<GameSetup>();
        GameSetup.GetInstance().InitGame();

        if (setupOnce)
            setupComplete = true;
        yield return null;
    }

    [UnityTest]
    public IEnumerator _001_InventoryManagerInitialized()
    {
        var inventoryPanel = UIManager.GetFromCanvas("Inventory");
        Assert.IsNotNull(inventoryPanel);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _002_InventoryCanBeToggled()
    {
        var inventoryOpen = InventoryManager.IsOpen;
        InventoryManager.ToggleInventory();
        Assert.IsTrue(inventoryOpen != InventoryManager.IsOpen);
        InventoryManager.ToggleInventory();
        Assert.IsTrue(inventoryOpen == InventoryManager.IsOpen);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _003_CanAddWordToInventory()
    {
        string word = "TestWordAdd";
        InventoryManager.AddWord(word);
        var inventoryWord = GameObject.FindObjectsOfType<InventoryWord>();
        Assert.IsNotNull(inventoryWord);
        bool wordFound = false;
        for(int i = 0; i < inventoryWord.Length; i++)
        {
            if(inventoryWord[i].name == word)
            {
                wordFound = true;
                break;
            }
        }
        Assert.IsTrue(wordFound);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _004_CanRemoveWordFromInventory()
    {
        string word = "TestWordRemove";
        InventoryManager.AddWord(word);
        InventoryManager.RemoveWord(word);
        var words = GameObject.FindObjectsOfType<InventoryWord>();
        var wordNotRemoved = false;
        for(int i = 0; i < words.Length; i++)
        {
            if (words[i].name == word)
            {
                wordNotRemoved = true;
                break;
            }
        }
        Assert.IsFalse(wordNotRemoved);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _005_CanGetWordsFromInventory()
    {
        string word1 = "TestWord1";
        string word2 = "TestWord2";
        var wordCount = GameObject.FindObjectsOfType<InventoryWord>().Length;
        InventoryManager.AddWord(word1);
        InventoryManager.AddWord(word2);
        var words = InventoryManager.GetWords();
        Assert.AreEqual(wordCount + 2, words.Count);
        yield return null;
    }
}

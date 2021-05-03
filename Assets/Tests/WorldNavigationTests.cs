using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using WordHoarder.Gameplay.World;
using WordHoarder.Setup;

public class WorldNavigationTests
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
        GameSetup.GetInstance().InitializeMainGame();

        if (setupOnce)
            setupComplete = true;
        yield return null;
    }

    [UnityTest]
    public IEnumerator _001_CanSetupEnvironment()
    {
        var worldNavigations = GameObject.FindObjectsOfType<WorldNavigation>();
        for(int i = 0; i < worldNavigations.Length; i++)
        {
            WorldNavigation wn = worldNavigations[i];
            Button unlocked = wn.gameObject.transform.GetChild(0).GetComponent<Button>();
            Button locked = wn.gameObject.transform.GetChild(1).GetComponent<Button>();
            wn.SetupEnvironment();
            Assert.IsFalse(unlocked.interactable);
            Assert.IsTrue(locked.interactable);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator _002_CanUnlockEnvironment()
    {
        var worldNavigations = GameObject.FindObjectsOfType<WorldNavigation>();
        for (int i = 0; i < worldNavigations.Length; i++)
        {
            WorldNavigation wn = worldNavigations[i];
            Button unlocked = wn.gameObject.transform.GetChild(0).GetComponent<Button>();
            Button locked = wn.gameObject.transform.GetChild(1).GetComponent<Button>();
            wn.SetupEnvironment();
            wn.UnlockEnvironment();
            Assert.IsFalse(locked.gameObject.activeInHierarchy);
            Assert.IsTrue(unlocked.gameObject.activeInHierarchy);
            Assert.IsTrue(unlocked.interactable);
        }
        yield return null;
    }

    [UnityTest]
    public IEnumerator _003_CanPrepareSaveData()
    {
        var worldNavigation = GameObject.FindObjectOfType<WorldNavigation>();
        var environmentName = worldNavigation.name;
        var saveData = worldNavigation.PrepareSaveData();
        Assert.IsTrue(saveData.Item1 == environmentName && saveData.Item2 == false);
        worldNavigation.UnlockEnvironment();
        saveData = worldNavigation.PrepareSaveData();
        Assert.IsTrue(saveData.Item1 == environmentName && saveData.Item2 == true);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _004_CanLoadSaveData()
    {
        var worldNavigation = GameObject.FindObjectOfType<WorldNavigation>();
        var unlockedButton = worldNavigation.transform.GetChild(0).GetComponent<Button>();
        var lockedButton = worldNavigation.transform.GetChild(1).GetComponent<Button>();
        var isComplete = true;
        worldNavigation.LoadSaveData(isComplete);
        Assert.IsTrue(unlockedButton.interactable && unlockedButton.gameObject.activeInHierarchy);
        Assert.IsFalse(lockedButton.interactable && lockedButton.gameObject.activeInHierarchy);
        yield return null;
    }
}

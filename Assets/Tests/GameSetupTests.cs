using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Setup;
using WordHoarder.Resources;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Managers.Static.Gameplay;
using UnityEditor;
using WordHoarder.Gameplay.GameScenarios;

public class GameSetupTests
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
        yield return new WaitForSeconds(1f);
        if (setupOnce)
            setupComplete = true;
        yield return null;
    }


    [UnityTest]
    public IEnumerator _001_SetupManagerIsInitialized()
    {
        Assert.IsNotNull(GameSetup.GetInstance());
        yield return null;
    }

    [UnityTest]
    public IEnumerator _002_MainMenuIsLoaded()
    {
        yield return new WaitForSeconds(2f);
        Assert.IsTrue(SceneManager.GetActiveScene().buildIndex == 2);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _003_AssetsManagerInitialized()
    {
        var gameResources = GameObject.FindObjectOfType<GameResources>();
        Assert.IsNotNull(gameResources);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _004_LocalizationManagerInitialized()
    {
        LocalizationManager.SetActiveLanguage(1);
        Assert.IsTrue(LocalizationManager.CurrentlyActiveLanguage == LocalizationManager.ActiveLanguage.LT);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _005_GameplayManagersPrepared()
    {
        GameSetup.GetInstance().InitGame();

        var mainCanvas = GameObject.Find("MainCanvas");
        Assert.IsNotNull(mainCanvas);

        var inventory = UIManager.GetFromCanvas("Inventory");
        Assert.IsNotNull(inventory);

        var puzzleManager = UIManager.GetFromCanvas("InteractivePanel");
        Assert.IsNotNull(puzzleManager);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _006_TutorialPrepared()
    {
        GameSetup.GetInstance().InitializeTutorial();
        var tutorial = GameObject.FindObjectOfType<TutorialScenario>();
        Assert.IsNotNull(tutorial);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _007_MainGamePrepared()
    {
        GameSetup.GetInstance().InitializeMainGame();
        var gameplay = GameObject.FindObjectOfType<GameScenario>();
        Assert.IsNotNull(gameplay);
        yield return null;
    }

}

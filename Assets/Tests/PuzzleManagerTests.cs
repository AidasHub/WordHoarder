using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Gameplay.Puzzles;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;

public class PuzzleManagerTests
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
    public IEnumerator _001_PuzzleManagerInitialized()
    {
        var puzzlePanel = UIManager.GetFromCanvas("InteractivePanel");
        Assert.IsNotNull(puzzlePanel);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _002_PuzzlePanelCanBeOpened()
    {
        bool puzzlePanelOpen = PuzzleManager.InteractivePanelOpen;
        PuzzleManager.ToggleInteraction();
        Assert.IsTrue(puzzlePanelOpen != PuzzleManager.InteractivePanelOpen);
        PuzzleManager.ToggleInteraction();
        Assert.IsTrue(puzzlePanelOpen == PuzzleManager.InteractivePanelOpen);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _003_WordFillPuzzleCanBeLoaded()
    {
        var puzzlePanel = UIManager.GetFromCanvas("InteractivePanel");
        PuzzleManager.LoadWordFillPuzzle(0, null);
        var wordFillGO = puzzlePanel.transform.gameObject.GetComponentInChildren<PuzzleWordFill>().gameObject;
        Assert.IsTrue(wordFillGO.activeInHierarchy);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _004_ImageGuessPuzzleCanBeLoaded()
    {
        var puzzlePanel = UIManager.GetFromCanvas("InteractivePanel");
        PuzzleManager.LoadImageGuessPuzzle(0, null);
        var imageGuessGO = puzzlePanel.transform.gameObject.GetComponentInChildren<PuzzleImageGuess>().gameObject;
        Assert.IsTrue(imageGuessGO.activeInHierarchy);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _005_WordFillPuzzleCanBeLoaded()
    {
        var puzzlePanel = UIManager.GetFromCanvas("InteractivePanel");
        PuzzleManager.LoadRotatingLockPuzzle(0, null);
        var rotatingLockGO = puzzlePanel.transform.gameObject.GetComponentInChildren<PuzzleRotatingLock>().gameObject;
        Assert.IsTrue(rotatingLockGO.activeInHierarchy);
        yield return null;
    }
}

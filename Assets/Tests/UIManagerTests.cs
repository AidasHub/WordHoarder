using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;

public class UIManagerTests
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

        if (setupOnce)
            setupComplete = true;
        yield return null;
    }

    [UnityTest]
    public IEnumerator _001_MainCanvasAdded()
    {
        var canvas = GameObject.Find("MainCanvas");
        Assert.IsNotNull(canvas);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _002_CanAddObjectsToCanvas()
    {
        GameObject test = new GameObject("TestInCanvas");
        var addedGO = UIManager.AddToCanvas(test);
        Assert.IsNotNull(addedGO);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _003_CanRetrieveObjectsFromCanvas()
    {
        GameObject test = new GameObject("TestInCanvas");
        UIManager.AddToCanvas(test);
        var retrievedGO = UIManager.GetFromCanvas("TestInCanvas");
        Assert.IsNotNull(retrievedGO);
        yield return null;
    }
}

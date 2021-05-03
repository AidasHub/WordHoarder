using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;
using TMPro;
public class TooltipManagerTests
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
    public IEnumerator _001_TooltipsAreShown()
    {
        TooltipManager.DrawTooltip("TooltipText");
        var tooltipGO = UIManager.GetFromCanvas("Tooltip");
        Assert.IsNotNull(tooltipGO);
        Assert.IsTrue(tooltipGO.activeInHierarchy);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _002_TooltipTextIsCorrect()
    {
        TooltipManager.DrawTooltip("TooltipText");
        var tooltipGO = UIManager.GetFromCanvas("Tooltip");
        Assert.AreEqual("TooltipText", tooltipGO.GetComponentInChildren<TextMeshProUGUI>().text);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _003_TooltipsGetHidden()
    {
        TooltipManager.DrawTooltip("TooltipText");
        var tooltipGO = UIManager.GetFromCanvas("Tooltip");
        Assert.IsTrue(tooltipGO.activeInHierarchy);
        TooltipManager.HideTooltip();
        Assert.IsFalse(tooltipGO.activeInHierarchy);
        yield return null;
    }
}

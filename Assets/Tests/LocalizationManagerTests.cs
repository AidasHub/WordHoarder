using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;

public class LocalizationManagerTests
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
    public IEnumerator _001_CanSetActiveLanguage()
    {
        var activeLanguage = LocalizationManager.CurrentlyActiveLanguage;
        Assert.IsTrue(activeLanguage == LocalizationManager.ActiveLanguage.EN);
        LocalizationManager.SetActiveLanguage(1);
        Assert.IsTrue(LocalizationManager.CurrentlyActiveLanguage == LocalizationManager.ActiveLanguage.LT);
        yield return null;        
    }

    [UnityTest]
    public IEnumerator _002_ReturnsCorrectLanguageStrings()
    {
        LocalizationManager.SetActiveLanguage(0);
        var activeLanguage = LocalizationManager.GetActiveLanguage();
        string testStringEN = activeLanguage.MiscContinue;
        LocalizationManager.SetActiveLanguage(1);
        activeLanguage = LocalizationManager.GetActiveLanguage();
        string testStringLT = activeLanguage.MiscContinue;
        Assert.AreEqual(testStringEN, "Continue");
        Assert.AreEqual(testStringLT, "Tęsti");
        yield return null;
    }
}

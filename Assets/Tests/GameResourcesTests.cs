using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Resources;
using WordHoarder.Setup;
using WordHoarder.Managers.Static.Gameplay;

public class GameResourcesTests
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

        if(GameSetup.GetInstance() != null)
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
    public IEnumerator _001_GameResourcesInitialized()
    {
        Assert.IsNotNull(GameResources.GetInstance());
        yield return null;
    }

    [UnityTest]
    public IEnumerator _002_GameResourcesDataInitialized()
    {
        Assert.IsNotNull(GameResources.Audio);
        Assert.IsNotNull(GameResources.Localization);
        Assert.IsNotNull(GameResources.ManagerPrefabs);
        Assert.IsNotNull(GameResources.Puzzles);
        Assert.IsNotNull(GameResources.Sprites);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _003_RetrievesAudioClipBySound()
    {
        var audioClip = GameResources.GetAudioClip(SoundManager.Sound.Test);
        Assert.IsNotNull(audioClip);
        yield return null;
    }

    [UnityTest]
    public IEnumerator _004_RetrievesSpriteByName()
    {
        var sprite = GameResources.GetSprite("Test");
        Assert.IsNotNull(sprite);
        yield return null;
    }
}

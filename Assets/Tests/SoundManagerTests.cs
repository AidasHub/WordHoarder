using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Setup;

public class SoundManagerTests
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
    public IEnumerator _001_PlaysSound()
    {
        SoundManager.PlaySound(SoundManager.Sound.Test);
        var audioPlayer = GameObject.FindObjectOfType<AudioSource>();
        Assert.IsTrue(audioPlayer.isPlaying);
        yield return null;
    }
}

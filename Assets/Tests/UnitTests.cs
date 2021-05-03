using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using WordHoarder.Setup;
using WordHoarder.Resources;

public class UnitTests
{
    public bool setupOnce = false;
    public bool setupComplete = false;

    //[UnitySetUp]
    public IEnumerator TestSetup()
    {
        if(!setupComplete)
        {
            var testSceneOperation = SceneManager.LoadSceneAsync("TestScene");
            while (!testSceneOperation.isDone)
                yield return null;
        }
        if (setupOnce)
            setupComplete = true;
        yield return null;
    }

}

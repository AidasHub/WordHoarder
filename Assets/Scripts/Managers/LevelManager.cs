using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static void LoadSplashScreen()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public static void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public static void LoadTutorial()
    {
        SceneManager.LoadSceneAsync(3);
        GameManager.getInstance().InitializeTutorial();
        SceneManager.sceneLoaded += UIManager.RefreshCanvasOnLevelLoad;
    }

    public static void LoadNewGame()
    {
        SceneManager.LoadSceneAsync(4);
        GameManager.getInstance().InitializeMainGame();
        SceneManager.sceneLoaded += UIManager.RefreshCanvasOnLevelLoad;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WordHoarder.Managers.Instanced;
using WordHoarder.Managers.Static.UI;
using static WordHoarder.Utility.SaveManager;

namespace WordHoarder.Utility
{

    public static class LevelManager
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
            _SetupManager.getInstance().InitializeTutorial();
            SceneManager.sceneLoaded += UIManager.RefreshCanvasOnLevelLoad;
        }

        public static void LoadNewGame()
        {
            SceneManager.LoadSceneAsync(4);
            _SetupManager.getInstance().InitializeMainGame();
            SceneManager.sceneLoaded += UIManager.RefreshCanvasOnLevelLoad;
        }

        public static void LoadExistingGame(SaveData saveData)
        {
            SceneManager.LoadSceneAsync(4);
            _SetupManager.getInstance().InitializeMainGame(saveData);
            SceneManager.sceneLoaded += UIManager.RefreshCanvasOnLevelLoad;
        }
    }
}
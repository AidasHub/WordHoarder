using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WordHoarder.Gameplay.GameScenarios;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Utility;
using WordHoarder.Resources;
using static WordHoarder.Utility.SaveUtility;

namespace WordHoarder.Setup
{
    public class GameSetup : MonoBehaviour
    {
        [SerializeField]
        private GameObject tutorialScenarioPrefab;
        [SerializeField]
        private GameObject gameScenarioPrefab;
        [SerializeField]
        private GameObject pauseMenuPrefab;
        [SerializeField]
        private GameObject wordBarPrefab;


        private static GameSetup instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(transform.parent);
                Init();
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        private void Init()
        {
            if (gameScenarioPrefab == null)
                gameScenarioPrefab = UnityEngine.Resources.Load("Prefabs/Gameplay/Game/GamePanel") as GameObject;
            if (tutorialScenarioPrefab == null)
                tutorialScenarioPrefab = UnityEngine.Resources.Load("Prefabs/Gameplay/Tutorial/TutorialPanel") as GameObject;
            if (pauseMenuPrefab == null)
                pauseMenuPrefab = UnityEngine.Resources.Load("Prefabs/UI/ESCMenu") as GameObject;
            if (wordBarPrefab == null)
                wordBarPrefab = UnityEngine.Resources.Load("Prefabs/UI/WordBar") as GameObject;

            GameObject assetsManager = new GameObject("AssetsManager");
            assetsManager.AddComponent<GameResources>();
            assetsManager.transform.parent = transform.parent;

            LocalizationManager.Init();
            SoundManager.Init();
            StartCoroutine(WaitASec()); // Required for automated UI testing
        }
        public IEnumerator WaitASec()
        {
            yield return new WaitForSeconds(1f);
            LevelManager.LoadSplashScreen();
        }

        public void InitGame()
        {
            UIManager.Init();
            InventoryManager.Init();
            PuzzleManager.Init();

            UIManager.AddToCanvas(pauseMenuPrefab, 0);
        }

        public static GameSetup GetInstance()
        {
            return instance;
        }

        public void InitializeTutorial()
        {
            UIManager.AddToCanvas(tutorialScenarioPrefab);
        }

        public void InitializeMainGame()
        {
            UIManager.AddToCanvas(gameScenarioPrefab, 0);
            UIManager.AddToCanvas(wordBarPrefab);
        }

        public void InitializeMainGame(SaveData data)
        {
            GameObject gameScenario = UIManager.AddToCanvas(gameScenarioPrefab, 0);
            UIManager.AddToCanvas(wordBarPrefab);
            gameScenario.GetComponent<GameScenario>().LoadSaveData(data);
        }

        public void ReturnToMainMenu()
        {
            for(int i = 0; i < transform.parent.childCount; i++)
            {
                var go = transform.parent.GetChild(i);
                if (!go.GetComponent<GameSetup>())
                    Destroy(transform.parent.GetChild(i).gameObject);
            }

            Init();
        }
    }
}
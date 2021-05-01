using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WordHoarder.Gameplay.World;
using WordHoarder.Gameplay.GameScenarios;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Utility;
using WordHoarder.Resources;
using static WordHoarder.Utility.SaveManager;

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

            gameScenario.GetComponent<GameScenario>().SwitchEnvironment(data.CurrentEnvironment);
            WorldNavigation[] environmentStatus = gameScenario.GetComponentsInChildren<WorldNavigation>(true);
            WorldWord[] worldWords = gameScenario.GetComponentsInChildren<WorldWord>(true);
            WorldInteractable[] reverseWords = gameScenario.GetComponentsInChildren<WorldInteractable>(true);

            if (environmentStatus.Length != data.EnvironmentStatus.Count)
            {
                Debug.LogError("Error loading a save file - environment navigation count does not match");
                return;
            }
            if (worldWords.Length != data.WorldWords.Count)
            {
                Debug.LogError("Error loading a save file - world words count does not match");
                return;
            }
            if(reverseWords.Length != data.ReverseWords.Count)
            {
                Debug.LogError("Error loading a save file - world interactable count does not match");
                return;
            }

            for (int i = 0; i < environmentStatus.Length; i++)
            {
                for (int j = 0; j < data.EnvironmentStatus.Count; j++)
                {
                    if (environmentStatus[i].gameObject.name == data.EnvironmentStatus[j].Item1)
                    {
                        environmentStatus[i].LoadSaveData(data.EnvironmentStatus[j].Item2);
                        break;
                    }
                }
            }

            for (int i = 0; i < worldWords.Length; i++)
            {
                for (int j = 0; j < data.WorldWords.Count; j++)
                {
                    if (worldWords[i].gameObject.name == data.WorldWords[j].Item1)
                    {
                        worldWords[i].LoadSaveData(data.WorldWords[j].Item2);
                        break;
                    }
                }
            }

            for(int i = 0; i < reverseWords.Length; i++)
            {
                for(int j = 0; j < data.ReverseWords.Count; j++)
                {
                    if(reverseWords[i].gameObject.name == data.ReverseWords[j].Item1)
                    {
                        reverseWords[i].LoadSaveData(data.ReverseWords[j].Item2);
                        break;
                    }
                }
            }

            for (int i = 0; i < data.InventoryWords.Count; i++)
            {
                InventoryManager.AddWord(data.InventoryWords[i]);
            }

            GameManager.ClearCollectedWords();
            for (int i = 0; i < data.CollectedWords; i++)
            {
                GameManager.IncreaseCollectedWords();
            }

            Debug.Log("Data successfully loaded!");
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class _SetupManager : MonoBehaviour
{
    public static bool isDoneLoadingGame;

    [SerializeField]
    private GameObject tutorialScenarioPrefab;
    [SerializeField]
    private GameObject gameScenarioPrefab;
    [SerializeField]
    private GameObject pauseMenuPrefab;
    [SerializeField]
    private GameObject wordBarPrefab;


    private static _SetupManager instance;

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
        GameObject assetsManager = new GameObject("AssetsManager");
        assetsManager.AddComponent<AssetsManager>();
        assetsManager.transform.parent = transform.parent;

        LocalizationManager.Init();

        LevelManager.LoadSplashScreen();
    }

    public void InitGame()
    {
        isDoneLoadingGame = false;

        UIManager.Init();
        InventoryManager.Init();
        InteractiveManager.Init();

        UIManager.AddToCanvas(pauseMenuPrefab, 0);
        isDoneLoadingGame = true;
    }

    public static _SetupManager getInstance()
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
        EnvironmentNavigation[] environmentStatus = gameScenario.GetComponentsInChildren<EnvironmentNavigation>(true);
        WorldWord[] worldWords = gameScenario.GetComponentsInChildren<WorldWord>(true);

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

        for(int i = 0; i < environmentStatus.Length; i++)
        {
            for(int j = 0; j < data.EnvironmentStatus.Count; j++)
            {
                if(environmentStatus[i].gameObject.name == data.EnvironmentStatus[j].Item1)
                {
                    environmentStatus[i].LoadSaveData(data.EnvironmentStatus[j].Item2);
                    break;
                }
            }
        }

        for(int i = 0; i < worldWords.Length; i++)
        {
            for(int j = 0; j < data.WorldWords.Count; j++)
            {
                if(worldWords[i].gameObject.name == data.WorldWords[j].Item1)
                {
                    worldWords[i].LoadSaveData(data.WorldWords[j].Item2);
                    break;
                }
            }
        }

        for(int i = 0; i < data.InventoryWords.Count; i++)
        {
            InventoryManager.AddWord(data.InventoryWords[i]);
        }

        GameManager.ClearCollectedWords();
        for(int i = 0; i < data.CollectedWords; i++)
        {
            GameManager.IncreaseCollectedWords();
        }

        Debug.Log("Data successfully loaded!");
    }
}

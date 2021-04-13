using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class _SetupManager : MonoBehaviour
{
    public static bool isDoneLoadingGame;

    [SerializeField]
    private List<GameObject> managerList;
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
        LocalizationManager.Init();
        LevelManager.LoadSplashScreen();
    }

    public void InitGame()
    {
        isDoneLoadingGame = false;
        for (int i = 0; i < managerList.Count; i++)
        {
            if (managerList[i])
            {
                Debug.Log("Instantiating manager");
                var manager = Instantiate(managerList[i], transform.parent);
                manager.name = managerList[i].name;
            }
        }
        UIManager.getInstance().AddToCanvas(pauseMenuPrefab, 0);
        isDoneLoadingGame = true;
    }

    public static _SetupManager getInstance()
    {
        return instance;
    }

    public void InitializeTutorial()
    {
        UIManager.getInstance().AddToCanvas(tutorialScenarioPrefab);
    }

    public void InitializeMainGame()
    {
        UIManager.getInstance().AddToCanvas(gameScenarioPrefab, 0);
        UIManager.getInstance().AddToCanvas(wordBarPrefab);
    }

    public void InitializeMainGame(SaveData data)
    {
        GameObject gameScenario = UIManager.getInstance().AddToCanvas(gameScenarioPrefab, 0);
        UIManager.getInstance().AddToCanvas(wordBarPrefab);

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
            InventoryManager.getInstance().AddWord(data.InventoryWords[i]);
        }

        GameManager.ClearCollectedWords();
        for(int i = 0; i < data.CollectedWords; i++)
        {
            GameManager.IncreaseCollectedWords();
        }

        Debug.Log("Data successfully loaded!");
    }
}

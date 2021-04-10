using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool debugMode;

    private static GameManager instance;
    public static bool isDoneLoadingGame;
    private static bool isGamePaused;
    public static bool GamePaused
    {
        get
        {
            return isGamePaused;
        }
        set
        {
            if (value == true)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
            isGamePaused = value;
        }
    }

    [SerializeField]
    private List<GameObject> managerList;

    [SerializeField]
    private GameObject tutorialScenarioPrefab;

    [SerializeField]
    private GameObject gameScenarioPrefab;

    private List<GameObject> activeManagerList;

    private void Awake()
    {
        isGamePaused = false;
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
        if(!debugMode)
            LevelManager.LoadSplashScreen();
    }

    public void InitGame()
    {
        activeManagerList = new List<GameObject>();
        isDoneLoadingGame = false;
        for (int i = 0; i < managerList.Count; i++)
        {
            if (managerList[i])
            {
                Debug.Log("Instantiating manager");
                var manager = Instantiate(managerList[i], transform.parent);
                manager.name = managerList[i].name;
                activeManagerList.Add(manager);
            }
        }
        isDoneLoadingGame = true;
    }

    public static GameManager getInstance()
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
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }
}

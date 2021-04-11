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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static bool isDone;

    [SerializeField]
    private List<GameObject> managerList;

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
        isDone = false;
        for (int i = 0; i < managerList.Count; i++)
        {
            if (managerList[i])
            {
                var manager = Instantiate(managerList[i], transform.parent);
                manager.name = managerList[i].name;
            }
        }
        isDone = true;
    }

    public static GameManager getInstance()
    {
        return instance;
    }
}

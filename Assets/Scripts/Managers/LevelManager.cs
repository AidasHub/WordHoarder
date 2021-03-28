using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    private UIManager _UIManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(1);
            SceneManager.sceneLoaded += LoadSplashScreen;
        }
    }

    public static LevelManager getInstance()
    {
        return instance;
    }

    private void LoadSplashScreen(Scene scene, LoadSceneMode mode)
    {
        _UIManager = UIManager.getInstance();
        StartCoroutine(OnSplashScreenLoaded());
        SceneManager.sceneLoaded -= LoadSplashScreen;

    }

    public IEnumerator OnSplashScreenLoaded()
    {
        yield return new WaitForSeconds(3f);
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(2);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvasPrefab;

    private GameObject mainCanvasGO;
    private Canvas mainCanvas;

    private LevelManager levelManager;

    private static UIManager instance;

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
        levelManager = LevelManager.getInstance();

        mainCanvasGO = Instantiate(canvasPrefab);
        mainCanvasGO.name = canvasPrefab.name;
        DontDestroyOnLoad(mainCanvasGO.gameObject);
        mainCanvas = mainCanvasGO.GetComponent<Canvas>();
    }

    public static UIManager getInstance()
    {
        return instance;
    }

    public GameObject AddToCanvas(GameObject go)
    {
        var ret = Instantiate(go, mainCanvasGO.transform);
        ret.name = go.name;
        return ret;
    }

    public GameObject getFromCanvas(string name)
    {
        return mainCanvas.transform.Find(name).gameObject;
    }
}

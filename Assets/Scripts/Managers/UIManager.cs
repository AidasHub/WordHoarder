using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject canvasPrefab;

    private GameObject mainCanvasGO;
    private Canvas mainCanvas;

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
        mainCanvasGO = Instantiate(canvasPrefab);
        mainCanvasGO.name = canvasPrefab.name;
        DontDestroyOnLoad(mainCanvasGO.gameObject);
        mainCanvas = mainCanvasGO.GetComponent<Canvas>();
        mainCanvas.worldCamera = Camera.main;
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

    public GameObject AddToCanvas(GameObject go, int indexInHierarchy)
    {
        if (indexInHierarchy >= mainCanvasGO.transform.childCount)
            return AddToCanvas(go);
        else
        {
            var ret = Instantiate(go, mainCanvasGO.transform);
            ret.name = go.name;
            ret.transform.SetSiblingIndex(indexInHierarchy);
            return ret;
        }
    }

    public GameObject GetFromCanvas(string name)
    {
        var findTransform = mainCanvasGO.transform.Find(name);
        if (findTransform != null)
            return findTransform.gameObject;
        else
            return null;

    }

    public static void RefreshCanvasOnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        instance.mainCanvas.worldCamera = Camera.main;
    }
}

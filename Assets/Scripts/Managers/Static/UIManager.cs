using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{

    private static GameObject canvasPrefab;
    private static GameObject mainCanvasGO;
    private static Canvas mainCanvas;

    public static void Init()
    {
        if(mainCanvasGO == null)
        {
            canvasPrefab = AssetsManager.ManagerPrefabs.MainCanvas;

            mainCanvasGO = GameObject.Instantiate(canvasPrefab);
            mainCanvasGO.name = canvasPrefab.name;
            GameObject.DontDestroyOnLoad(mainCanvasGO.gameObject);
            mainCanvas = mainCanvasGO.GetComponent<Canvas>();
            mainCanvas.worldCamera = Camera.main;
        }
    }

    public static GameObject AddToCanvas(GameObject go)
    {
        if(mainCanvasGO == null)
        {
            Init();
        }
        var ret = GameObject.Instantiate(go, mainCanvasGO.transform);
        ret.name = go.name;
        return ret;
    }

    public static GameObject AddToCanvas(GameObject go, int indexInHierarchy)
    {
        if(mainCanvasGO == null)
        {
            Init();
        }
        if (indexInHierarchy >= mainCanvasGO.transform.childCount)
            return AddToCanvas(go);
        else
        {
            var ret = GameObject.Instantiate(go, mainCanvasGO.transform);
            ret.name = go.name;
            ret.transform.SetSiblingIndex(indexInHierarchy);
            return ret;
        }
    }

    public static GameObject GetFromCanvas(string name)
    {
        if(mainCanvasGO == null)
        {
            Init();
        }
        var findTransform = mainCanvasGO.transform.Find(name);
        if (findTransform != null)
            return findTransform.gameObject;
        else
            return null;

    }

    public static void RefreshCanvasOnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        if(mainCanvas == null)
        {
            Init();
        }
        mainCanvas.worldCamera = Camera.main;
    }
}

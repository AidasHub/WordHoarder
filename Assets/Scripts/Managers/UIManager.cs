using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject canvasPrefab;
    [SerializeField]
    private GameObject ESCMenuPrefab;

    [Header("GameObjects")]
    private GameObject interactivePanel;
    private GameObject inventoryPanel;

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
        interactivePanel = mainCanvasGO.transform.Find("InteractionPanel").gameObject;
        inventoryPanel = mainCanvasGO.transform.Find("Inventory").gameObject;
        AddToCanvas(ESCMenuPrefab);
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

    public GameObject getFromCanvas(string name)
    {
        return mainCanvas.transform.Find(name).gameObject;
    }

    public static void RefreshCanvasOnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        instance.mainCanvas.worldCamera = Camera.main;
    }

    public GameObject GetInventory()
    {
        return inventoryPanel;
    }
    
    public GameObject GetInteractionPanel()
    {
        return interactivePanel;
    }
}

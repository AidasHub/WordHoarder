using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [SerializeField]
    private float offset = 220f;

    [SerializeField]
    private float openingSpeed = 1f;

    private bool isOpen = false;
    //private bool isMoving = false;
    private bool queueClose = false;

    [SerializeField]
    private GameObject wordPrefab;

    [SerializeField]
    private GameObject inventory;
    private Animator inventoryAnimator;
    private Button inventoryButton;
    private GridLayoutGroup wordGridLayout;
    private RectTransform rectTransform;

    [SerializeField]
    List<InventoryWord> wordList;

    private static InventoryManager instance;

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

    public static InventoryManager getInstance()
    {
        return instance;
    }

    private void Init()
    {
        Debug.Log("Inventory INIT");
        if(inventory == null)
        {
            inventory = UIManager.getInstance().getFromCanvas("Inventory");
            inventoryAnimator = inventory.GetComponent<Animator>();
            inventoryButton = inventory.GetComponentInChildren<Button>();
            inventoryButton.onClick.AddListener(ToggleInventory);
        }
        wordGridLayout = inventory.GetComponentInChildren<GridLayoutGroup>();
    }

    private void Start()
    {
        rectTransform = inventory.GetComponent<RectTransform>();
    }

    public void AddWord(string word)
    {
        GameObject newWordGO = Instantiate(wordPrefab, wordGridLayout.transform);
        newWordGO.name = word;
        InventoryWord newWord = newWordGO.GetComponent<InventoryWord>();
        TextMeshProUGUI newWordTMP = newWordGO.GetComponent<TextMeshProUGUI>();
        newWordTMP.text = word;
        wordList.Add(newWord);
    }

    public void RemoveWord(string word)
    {
        for(int i = 0; i < wordList.Count; i++)
        {
            if (wordList[i].getWordString() == word)
            {
                wordList.RemoveAt(i);
                RefreshInventory();
                break;
            }          
        }
    }

    public void RefreshInventory()
    {
        for(int i = 0; i < wordGridLayout.transform.childCount; i++)
        {
            bool found = false;
            for (int j = 0; j < wordList.Count; j++)
            {
                if (wordGridLayout.transform.GetChild(i).name == wordList[j].getWordString())
                    found = true;
            }
            if (!found)
            {
                Destroy(wordGridLayout.transform.GetChild(i).gameObject);
            }
        }
    }

    public List<InventoryWord> GetWords()
    {
        return wordList;
    }

    public void ToggleInventory()
    {
        Debug.Log(inventoryAnimator == null);
        inventoryAnimator.SetTrigger("ToggleInventory");
        Debug.Log("Toggled Inventory");
        if (!isOpen)
        {
            Debug.Log("Opening");
            //StartCoroutine(InventoryTransformation());
        }
        else
        {
            Debug.Log("Closing");
            //StartCoroutine(InventoryTransformation());
        }
    }

    IEnumerator InventoryTransformation()
    {
        if (!isOpen)
        {
            //isMoving = true;
            while (rectTransform.offsetMin.y <= 0 && rectTransform.offsetMax.y <= 0)
            {
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y + openingSpeed * Time.deltaTime * 100);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y + openingSpeed * Time.deltaTime * 100);
                yield return null;
            }
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0f);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0f);
            //isMoving = false;
            isOpen = true;
            if (queueClose)
            {
                queueClose = false;
                //StartCoroutine(InventoryTransformation());
            }
        }
        else
        {
            //isMoving = true;
            while(rectTransform.offsetMin.y >= -offset && rectTransform.offsetMax.y >= -offset)
            {
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y - openingSpeed * Time.deltaTime * 100);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y - openingSpeed * Time.deltaTime * 100);
                yield return null;
            }
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -offset);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -offset);
            //isMoving = false;
            isOpen = false;
        }
    }
}

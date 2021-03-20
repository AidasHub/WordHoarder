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
    private bool isMoving = false;
    private bool queueClose = false;

    [SerializeField]
    private GameObject wordPrefab;

    [SerializeField]
    private GameObject inventory;
    private GridLayoutGroup wordGridLayout;
    private RectTransform rectTransform;

    [SerializeField]
    List<Word_Inventory> wordList;

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

    private void Init()
    {
        wordGridLayout = inventory.GetComponentInChildren<GridLayoutGroup>();
    }

    private void Start()
    {
        rectTransform = inventory.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddWord("Computer");
        }
    }

    public void AddWord(string word)
    {
        GameObject newWordGO = Instantiate(wordPrefab, wordGridLayout.transform);
        Word_Inventory newWord = newWordGO.GetComponent<Word_Inventory>();
        TextMeshProUGUI newWordTMP = newWordGO.GetComponent<TextMeshProUGUI>();
        newWordTMP.text = word;
        wordList.Add(newWord);
    }

    bool isFullyVisible(Rect rect)
    {
        Rect screenBounds = wordGridLayout.gameObject.GetComponent<RectTransform>().rect;
        if (screenBounds.Contains(new Vector2(rect.xMin, rect.yMin))
            && screenBounds.Contains(new Vector2(rect.xMin, rect.yMax))
            && screenBounds.Contains(new Vector2(rect.xMax, rect.yMin))
            && screenBounds.Contains(new Vector2(rect.xMax, rect.yMax)))
            return true;
        return false;
    }


    public void ToggleInventory()
    {
        if (!isOpen)
        {
            StartCoroutine(InventoryTransformation());
        }
        else
        {
            StartCoroutine(InventoryTransformation());
        }
    }

    IEnumerator InventoryTransformation()
    {
        if (!isOpen)
        {
            isMoving = true;
            while (rectTransform.offsetMin.y <= 0 && rectTransform.offsetMax.y <= 0)
            {
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y + openingSpeed * Time.deltaTime * 100);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y + openingSpeed * Time.deltaTime * 100);
                yield return null;
            }
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0f);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 0f);
            isMoving = false;
            isOpen = true;
            if (queueClose)
            {
                queueClose = false;
                //StartCoroutine(InventoryTransformation());
            }
        }
        else
        {
            isMoving = true;
            while(rectTransform.offsetMin.y >= -offset && rectTransform.offsetMax.y >= -offset)
            {
                rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, rectTransform.offsetMin.y - openingSpeed * Time.deltaTime * 100);
                rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y - openingSpeed * Time.deltaTime * 100);
                yield return null;
            }
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, -offset);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -offset);
            isMoving = false;
            isOpen = false;
        }
    }
}

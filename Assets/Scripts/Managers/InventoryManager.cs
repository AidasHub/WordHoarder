using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryPanelPrefab;

    public bool IsOpen { get; private set; } = false;

    [SerializeField]
    private GameObject wordPrefab;

    [SerializeField]
    private GameObject inventory;
    private Animator inventoryAnimator;
    private Button inventoryButton;
    private GridLayoutGroup wordGridLayout;

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
        if(inventory == null)
        {
            inventory = UIManager.getInstance().AddToCanvas(inventoryPanelPrefab);
            inventoryAnimator = inventory.GetComponent<Animator>();
            inventoryButton = inventory.GetComponentInChildren<Button>();
            inventoryButton.onClick.AddListener(ToggleInventory);
        }
        wordGridLayout = inventory.GetComponentInChildren<GridLayoutGroup>();
    }

    public void AddWord(string word)
    {
        GameObject newWordGO = Instantiate(wordPrefab, wordGridLayout.transform);
        newWordGO.name = word;
        InventoryWord newWord = newWordGO.GetComponent<InventoryWord>();
        TextMeshProUGUI newWordTMP = newWordGO.GetComponent<TextMeshProUGUI>();
        newWordTMP.text = word;
        wordList.Add(newWord);
        GameManager.IncreaseCollectedWords();
    }

    public void RemoveWord(string word)
    {
        for(int i = 0; i < wordList.Count; i++)
        {
            if (wordList[i].getWordString().ToLower() == word.ToLower())
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
        if (IsOpen)
        {
            IsOpen = false;
            inventoryAnimator.SetBool("InventoryOpen", false);
        }
        else
        {
            Debug.Log("Opening");
            IsOpen = true;
            inventoryAnimator.SetBool("InventoryOpen", true);
        }
    }

    public GameObject GetInventoryGO()
    {
        return inventory;
    }

    public void EnableInventory()
    {
        inventory.SetActive(true);
    }

    public void DisableInventory()
    {
        inventory.SetActive(false);
    }

}

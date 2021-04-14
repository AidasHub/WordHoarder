using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : MonoBehaviour
{

    public static ManagerPrefabResource ManagerPrefabs { get; private set; }
    public static WordResource Words { get; private set; }
    public static PuzzleResource Puzzles { get; private set; }
    public static LocalizationResource Localization { get; private set; }

    private static AssetsManager instance;

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

    private static void Init()
    {
        ManagerPrefabs = new ManagerPrefabResource();
        ManagerPrefabs.MainCanvas = Resources.Load("Prefabs/Managers/MainCanvas") as GameObject;
        ManagerPrefabs.Inventory = Resources.Load("Prefabs/Managers/Inventory") as GameObject;
        ManagerPrefabs.InteractivePanel = Resources.Load("Prefabs/Managers/InteractivePanel") as GameObject;

        Words = new WordResource();
        Words.InventoryWord = Resources.Load("Prefabs/Inventory/InventoryWord") as GameObject;

        Puzzles = new PuzzleResource();
        List<TextAsset> wordFillPuzzleAssets = new List<TextAsset>();
        TextAsset[] textAssets = Resources.LoadAll<TextAsset>("Puzzles/WordFill/");
        for(int i = 0; i < textAssets.Length; i++)
        {
            wordFillPuzzleAssets.Add(textAssets[i]);
        }
        Puzzles.WordFillPuzzles = wordFillPuzzleAssets;

        Localization = new LocalizationResource();
        Localization.Languages = Resources.LoadAll<TextAsset>("Localization/");
    }

    [Serializable]
    public class ManagerPrefabResource
    {
        public GameObject MainCanvas;
        public GameObject Inventory;
        public GameObject InteractivePanel;
    }

    [Serializable]
    public class WordResource
    {
        public GameObject InventoryWord;
    }

    [Serializable]
    public class PuzzleResource
    {
        public List<TextAsset> WordFillPuzzles;
    }

    [Serializable]
    public class LocalizationResource
    {
        public TextAsset[] Languages;
    }
}

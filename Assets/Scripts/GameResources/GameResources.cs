using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordHoarder.Managers.Static.Gameplay;

namespace WordHoarder.Resources
{
    public class GameResources : MonoBehaviour
    {

        public static ManagerPrefabResource ManagerPrefabs { get; private set; }
        public static WordResource Words { get; private set; }
        public static PuzzleResource Puzzles { get; private set; }
        public static LocalizationResource Localization { get; private set; }

        public static List<AudioResource> Audio { get; private set; }

        public static List<Sprite> Sprites { get; private set; }

        private static GameResources instance;

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

        public static GameResources GetInstance()
        {
            return instance;
        }

        private static void Init()
        {
            ManagerPrefabs = new ManagerPrefabResource();
            ManagerPrefabs.MainCanvas = UnityEngine.Resources.Load("Prefabs/Managers/MainCanvas") as GameObject;
            ManagerPrefabs.Inventory = UnityEngine.Resources.Load("Prefabs/Managers/Inventory") as GameObject;
            ManagerPrefabs.InteractivePanel = UnityEngine.Resources.Load("Prefabs/Managers/InteractivePanel") as GameObject;
            ManagerPrefabs.Tooltip = UnityEngine.Resources.Load("Prefabs/Managers/Tooltip") as GameObject;

            Words = new WordResource();
            Words.InventoryWord = UnityEngine.Resources.Load("Prefabs/Inventory/InventoryWord") as GameObject;

            Puzzles = new PuzzleResource();
            List<TextAsset> wordFillPuzzleAssets = new List<TextAsset>();
            TextAsset[] textAssets = UnityEngine.Resources.LoadAll<TextAsset>("Puzzles/WordFill/");
            for (int i = 0; i < textAssets.Length; i++)
            {
                wordFillPuzzleAssets.Add(textAssets[i]);
            }
            Puzzles.WordFillPuzzles = wordFillPuzzleAssets;

            List<TextAsset> rotatingLockPuzzleAssets = new List<TextAsset>();
            textAssets = UnityEngine.Resources.LoadAll<TextAsset>("Puzzles/RotatingLock/");
            for (int i = 0; i < textAssets.Length; i++)
            {
                rotatingLockPuzzleAssets.Add(textAssets[i]);
            }
            Puzzles.RotatingLockPuzzles = rotatingLockPuzzleAssets;

            List<TextAsset> imageGuessPuzzleAssets = new List<TextAsset>();
            textAssets = UnityEngine.Resources.LoadAll<TextAsset>("Puzzles/ImageGuess/");
            for (int i = 0; i < textAssets.Length; i++)
            {
                imageGuessPuzzleAssets.Add(textAssets[i]);
            }
            Puzzles.ImageGuessPuzzles = imageGuessPuzzleAssets;

            Localization = new LocalizationResource();
            Localization.Languages = UnityEngine.Resources.LoadAll<TextAsset>("Localization/");

            AudioClip[] audioAssets = UnityEngine.Resources.LoadAll<AudioClip>("Audio/");
            string[] soundNames = Enum.GetNames(typeof(SoundManager.Sound));
            Audio = new List<AudioResource>();
            for (int i = 0; i < audioAssets.Length; i++)
            {
                for (int j = 0; j < soundNames.Length; j++)
                {
                    if (audioAssets[i].name == soundNames[j])
                    {
                        AudioResource resource = new AudioResource();
                        resource.audioClip = audioAssets[i];
                        SoundManager.Sound sound;
                        Enum.TryParse<SoundManager.Sound>(soundNames[j], out sound);
                        resource.sound = sound;
                        Audio.Add(resource);
                        break;
                    }
                }
            }

            List<Sprite> spriteAssets = new List<Sprite>();
            Sprite[] sprites = UnityEngine.Resources.LoadAll<Sprite>("Sprites/");
            for (int i = 0; i < sprites.Length; i++)
            {
                spriteAssets.Add(sprites[i]);
            }
            Sprites = spriteAssets;
            Debug.Log(Sprites.Count);
        }

        public static AudioClip GetAudioClip(SoundManager.Sound sound)
        {
            for (int i = 0; i < Audio.Count; i++)
            {
                if (Audio[i].sound == sound)
                {
                    return Audio[i].audioClip;
                }
            }
            Debug.LogError("AudioClip for " + sound + " was not found");
            return null;
        }

        public static Sprite GetSprite(string name)
        {
            Debug.Log("Searching for " + name);
            for (int i = 0; i < Sprites.Count; i++)
            {
                if (Sprites[i].name == name)
                    return Sprites[i];
            }
            Debug.LogError("Sprite of the name " + name + " was not found");
            return null;
        }

        [Serializable]
        public class ManagerPrefabResource
        {
            public GameObject MainCanvas;
            public GameObject Inventory;
            public GameObject InteractivePanel;
            public GameObject Tooltip;
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
            public List<TextAsset> RotatingLockPuzzles;
            public List<TextAsset> ImageGuessPuzzles;
        }

        [Serializable]
        public class LocalizationResource
        {
            public TextAsset[] Languages;
        }

        [Serializable]
        public class AudioResource
        {
            public SoundManager.Sound sound;
            public AudioClip audioClip;
        }
    }
}

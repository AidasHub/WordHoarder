using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Events;
using WordHoarder.Managers.Static.UI;


namespace WordHoarder.Gameplay.Puzzles
{
    public class PuzzleWordFill : MonoBehaviour
    {
        private class PuzzleInfo
        {
            public string textEN;
            public string[] missingWordsEN;
            public string textOther;
            public string[] missingWordsOther;

            public static PuzzleInfo CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<PuzzleInfo>(jsonString);
            }
        }

        [Header("Puzzle Settings")]
        [SerializeField]
        private int dashCount = 10;
        [SerializeField]
        private GameObject blankWordPrefab;

        [Header("Puzzle children")]
        [SerializeField]
        private TextMeshProUGUI tmpLeft;
        [SerializeField]
        private TextMeshProUGUI tmpRight;
        [SerializeField]

        private GridLayoutGroup inventoryGridLayout;
        private List<string> usedWords = new List<string>();
        private UnityAction rewardAction;


        void Awake()
        {
            inventoryGridLayout = GetComponentInChildren<GridLayoutGroup>();
        }

        public void InitPuzzle(TextAsset puzzle, UnityAction rewardAction)
        {
            this.rewardAction = rewardAction;
            PopulateWordPanels(puzzle);
            PopulatePuzzleInventory();
        }

        public void Close()
        {
            //CleanupBlanks();
            CleanupInventory();
        }

        private void CleanupBlanks()
        {
            for (int i = 0; i < tmpLeft.transform.childCount; i++)
            {
                Destroy(tmpLeft.transform.GetChild(i).gameObject);
            }
            blankWords = new List<GameObject>();
        }

        private void CleanupInventory()
        {
            for (int i = 0; i < inventoryGridLayout.transform.childCount; i++)
            {
                Destroy(inventoryGridLayout.transform.GetChild(i).gameObject);
            }
        }


        public void UpdatePuzzle(string word, int wordIndex)
        {
            string replacement = "<color=#006400>" + word + "</color>";

            tmpLeft.text = ReplaceWord(tmpLeft.text, wordIndex - 1, replacement);
            for (int i = 0; i < blankWords.Count; i++)
            {
                if (blankWords[i].name == word)
                    blankWords.RemoveAt(i);
            }
            usedWords.Add(word);
            UpdateBlankWordPositions();
            if (blankWords.Count == 0)
            {
                CompletePuzzle();
            }
        }

        public string ReplaceWord(string text, int wordIndex, string replacement)
        {
            string[] words = text.Split(' ');
            string oldWord = words[wordIndex];
            string newWord = replacement;
            if (Char.IsPunctuation(oldWord[0]))
                newWord = newWord.Insert(0, oldWord[0].ToString());
            if (Char.IsPunctuation(oldWord[oldWord.Length - 1]))
                newWord = newWord.Insert(newWord.Length, oldWord[oldWord.Length - 1].ToString());

            string newText = "";

            for (int i = 0; i < words.Length; i++)
            {
                if (i == wordIndex)
                    newText += newWord + " ";
                else
                {
                    newText += words[i] + " ";
                }
            }

            newText.Remove(newText.Length - 1, 1);
            return newText;
        }

        public void PopulatePuzzleInventory()
        {
            List<InventoryWord> inventoryWords = InventoryManager.GetWords();
            Debug.Log(InventoryManager.GetWords().Count);
            foreach (InventoryWord word in inventoryWords)
            {
                var GO = Instantiate(word, inventoryGridLayout.transform);
                GO.name = word.name;
            }
        }

        private List<GameObject> blankWords = new List<GameObject>();

        public void PopulateWordPanels(TextAsset puzzle)
        {
            CleanupBlanks();
            PuzzleInfo puzzleInfo = PuzzleInfo.CreateFromJSON(puzzle.text);

            #region EN_SETUP

            string textEN = puzzleInfo.textEN;

            string dashReplacement = "|";
            for (int i = 0; i < dashCount; i++)
                dashReplacement += "|||";
            textEN = puzzleInfo.textEN.Replace("_", dashReplacement);


            tmpLeft.text = textEN;
            tmpLeft.ForceMeshUpdate();

            List<Tuple<int, int>> blanks = FindBlanksInText(tmpLeft.text, '|');

            for (int i = 0; i < blanks.Count; i++)
            {
                var firstCharIndex = blanks[i].Item1;
                var lastCharIndex = blanks[i].Item2 - 1;

                Vector3 botLeft = tmpLeft.textInfo.characterInfo[firstCharIndex].bottomLeft;
                Vector3 topLeft = tmpLeft.textInfo.characterInfo[firstCharIndex].topLeft;
                Vector3 topRight = tmpLeft.textInfo.characterInfo[lastCharIndex].topRight;
                Vector3 botRight = tmpLeft.textInfo.characterInfo[lastCharIndex].bottomRight;

                GameObject blankWord = Instantiate(blankWordPrefab, tmpLeft.gameObject.transform);
                blankWord.name = puzzleInfo.missingWordsEN[i];
                blankWord.GetComponent<WordSlot>().SetWord(puzzleInfo.missingWordsEN[i], FindWordIndex(tmpLeft.text, firstCharIndex));
                blankWords.Add(blankWord);

                blankWord.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(topLeft.x - topRight.x), Mathf.Abs(topLeft.y - botLeft.y));
                var offsetX = (topRight.x - topLeft.x) / 2;
                var offsetY = (topLeft.y - botLeft.y) / 2;
                blankWord.GetComponent<RectTransform>().anchoredPosition = new Vector2(topLeft.x + offsetX, botLeft.y + offsetY);
            }

            #endregion

            #region OTHER_SETUP

            List<Tuple<int, int>> blanksOther = FindBlanksInText(puzzleInfo.textOther, '_');

            string textOther = puzzleInfo.textOther;

            for (int i = blanksOther.Count - 1; i >= 0; i--)
            {
                textOther = textOther.Remove(blanksOther[i].Item1, 1);
                textOther = textOther.Insert(blanksOther[i].Item1, puzzleInfo.missingWordsOther[i]);
            }

            tmpRight.text = textOther;
            tmpRight.ForceMeshUpdate();


            #endregion
        }

        private void UpdateBlankWordPositions()
        {
            List<Tuple<int, int>> blanks = FindBlanksInText(tmpLeft.text, '|');
            tmpLeft.ForceMeshUpdate();
            Debug.Log(blanks.Count);

            for (int i = 0; i < blankWords.Count; i++)
            {
                var firstCharIndex = blanks[i].Item1;
                var lastCharIndex = blanks[i].Item2 - 1;

                Debug.Log(firstCharIndex);
                Debug.Log(lastCharIndex);

                Vector3 botLeft = tmpLeft.textInfo.characterInfo[firstCharIndex].bottomLeft;
                Vector3 topLeft = tmpLeft.textInfo.characterInfo[firstCharIndex].topLeft;
                Vector3 topRight = tmpLeft.textInfo.characterInfo[lastCharIndex].topRight;
                Vector3 botRight = tmpLeft.textInfo.characterInfo[lastCharIndex].bottomRight;

                blankWords[i].GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(topLeft.x - topRight.x), Mathf.Abs(topLeft.y - botLeft.y));
                var offsetX = (topRight.x - topLeft.x) / 2;
                var offsetY = (topLeft.y - botLeft.y) / 2;
                blankWords[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(topLeft.x + offsetX, botLeft.y + offsetY);
            }
        }


        private List<Tuple<int, int>> FindBlanksInText(string text, char blankChar)
        {
            List<Tuple<int, int>> blanks = new List<Tuple<int, int>>();

            bool blankStarted = false;
            int blankStart = 0;
            int blankEnd = 0;
            int blankLength = 0;

            text = Regex.Replace(text, "<.*?>", "");

            for (int i = 0; i < text.Length; i++)
            {
                if (!blankStarted)
                {
                    if (text[i] == blankChar)
                    {
                        blankStarted = true;
                        blankStart = i;
                        blankLength = 1;
                    }
                }
                else //(blankStarted)
                {
                    if (text[i] == blankChar)
                    {
                        blankLength++;
                    }
                    else
                    {
                        blankEnd = blankStart + blankLength;
                        blanks.Add(new Tuple<int, int>(blankStart, blankEnd));
                        blankStarted = false;
                    }
                }
            }
            if (blankStarted)
            {
                blankEnd = text.Length;
                blanks.Add(new Tuple<int, int>(blankStart, blankEnd));
            }
            return blanks;
        }

        private int FindWordIndex(string text, int charIndex)
        {
            int wordIndex = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == ' ')
                    wordIndex++;
                if (i == charIndex)
                    break;
            }
            return wordIndex + 1;
        }

        private void CompletePuzzle()
        {
            rewardAction.Invoke();
            for (int i = 0; i < usedWords.Count; i++)
            {
                InventoryManager.RemoveWord(usedWords[i]);
            }
        }
    }
}
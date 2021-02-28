using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordPuzzle : MonoBehaviour
{

    private TextMeshProUGUI tmp;
    private Dictionary<string, int> missingWords;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        missingWords = new Dictionary<string, int>();
        missingWords.Add("breakfast", 15);
    }

    public void UpdatePuzzle(string word)
    {
        if (missingWords.ContainsKey(word))
        {
            string[] words = tmp.text.Split(' ');
            int wordIndex = missingWords[word];
            string wordsNew = "";
            int i = 0;
            while(i < wordIndex-1)
            {
                wordsNew += words[i] + " ";
                i++;
            }

            wordsNew += "<color=green>" + word + "</color>";
            i++;
            if (words[i].Length != 1)
                wordsNew += " ";

            while (i < words.Length)
            {
                wordsNew += words[i] + " ";
                i++;
            }
            wordsNew = wordsNew.Remove(wordsNew.Length - 1);

            tmp.text = wordsNew;
        }
    }

    public void AddWord(int wordIndex, string word)
    {
        missingWords.Add(word, wordIndex);
    }

}

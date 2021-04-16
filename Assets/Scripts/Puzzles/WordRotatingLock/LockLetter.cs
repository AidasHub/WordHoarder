using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RotatingLockLetter
{
    private char[] availableLetters;
    private TextMeshProUGUI activeLetter;
    public int CurrentLetterIndex { get; private set; }

    public RotatingLockLetter(char correctLetter, int randomLetterCount, TextMeshProUGUI letterText)
    {
        activeLetter = letterText;
        availableLetters = new char[randomLetterCount + 1];
        availableLetters[0] = correctLetter;

        for(int i = 1; i < availableLetters.Length; i++)
        {
            char incorrectLetter = (char)Random.Range(65, 91);
            while (ArrayContainsChar(availableLetters, incorrectLetter))
            {
                incorrectLetter = (char)Random.Range(65, 91);
            }
            availableLetters[i] = incorrectLetter;
        }
        CurrentLetterIndex = Random.Range(0, availableLetters.Length);
        UpdateActiveLetterDisplay();
    }

    private bool ArrayContainsChar(char[] array, char c)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == c)
                return true;
        }
        return false;
    }

    private void UpdateActiveLetterDisplay()
    {
        activeLetter.text = availableLetters[CurrentLetterIndex].ToString();
    }

    public void ChangeActiveLetter(bool isNext)
    {
        if (isNext)
        {
            CurrentLetterIndex++;
            if (CurrentLetterIndex >= availableLetters.Length)
                CurrentLetterIndex = 0;
        }
        else
        {
            CurrentLetterIndex--;
            if (CurrentLetterIndex < 0)
                CurrentLetterIndex = availableLetters.Length - 1;
        }
        UpdateActiveLetterDisplay();
    }

    public bool ActiveLetterIsCorrect()
    {
        return CurrentLetterIndex == 0;
    }
}

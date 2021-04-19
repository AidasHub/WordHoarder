using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class GameManager
{
    private static bool isGamePaused = false;
    public static bool GamePaused
    {
        get
        {
            return isGamePaused;
        }
        set
        {
            if (value == true)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
            isGamePaused = value;
        }
    }

    public static UnityEvent onWordCollected = new UnityEvent();

    private static int totalWords = 0;
    public static int TotalWords
    {
        get
        {
            return totalWords;
        }
        set
        {
            totalWords = value;
        }
    }

    public static int CollectedWords { get; private set; }

    public static void IncreaseCollectedWords()
    {
        CollectedWords++;
        onWordCollected.Invoke();
    }

    public static void ClearCollectedWords()
    {
        CollectedWords = 0;
    }
}

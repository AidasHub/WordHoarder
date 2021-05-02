using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WordHoarder.Setup;

namespace WordHoarder.Managers.Static.Gameplay
{
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

        public static int TotalWords { get; set; }

        public static int CollectedWords { get; private set; }

        public static void IncreaseCollectedWords()
        {
            CollectedWords++;
            onWordCollected.Invoke();
            if(CollectedWords == TotalWords)
            {
                SceneManager.LoadScene(5);
            }
        }

        public static void ClearCollectedWords()
        {
            CollectedWords = 0;
        }
    }
}
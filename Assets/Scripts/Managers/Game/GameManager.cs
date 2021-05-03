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

        public static UnityEvent onProgressMade = new UnityEvent();

        public static int TotalWords { get; set; }
        public static int CollectedWords { get; private set; }
        public static int TotalEnvironments { get; set; }
        public static int UnlockedEnvironments { get; set; }
        public static int TotalHiddenObjects { get; set; }
        public static int RevealedHiddenObjects { get; set; }

        public static void IncreaseCollectedWords()
        {
            CollectedWords++;
            onProgressMade.Invoke();
            CheckForCompletion();
        }
        public static void IncreaseUnlockedEnvironments()
        {
            UnlockedEnvironments++;
            onProgressMade.Invoke();
            CheckForCompletion();
        }
        public static void IncreaseRevealedHiddenObjects()
        {
            CollectedWords++;
            onProgressMade.Invoke();
            CheckForCompletion();
        }

        private static void CheckForCompletion()
        {
            if (CollectedWords == TotalWords && UnlockedEnvironments == TotalEnvironments && RevealedHiddenObjects == TotalHiddenObjects)
                SceneManager.LoadScene(5);
        }

        public static void ClearCollectedWords()
        {
            CollectedWords = 0;
            onProgressMade.Invoke();
        }

        public static void ClearUnlockedEnvironments()
        {
            UnlockedEnvironments = 0;
            onProgressMade.Invoke();
        }

        public static void ClearRevealedHiddenObjects()
        {
            RevealedHiddenObjects = 0;
            onProgressMade.Invoke();
        }

        public static void ResetProgress()
        {
            ClearCollectedWords();
            ClearUnlockedEnvironments();
            ClearRevealedHiddenObjects();
        }
    }
}
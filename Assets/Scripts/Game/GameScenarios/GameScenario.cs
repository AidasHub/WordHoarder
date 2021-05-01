using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WordHoarder.Gameplay.World;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;

namespace WordHoarder.Gameplay.GameScenarios
{
    public class GameScenario : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> environments;
        public int CurrentEnvironment { get; private set; }

        public void Awake()
        {
            GameManager.TotalWords = GetComponentsInChildren<WorldWord>(true).Length;
        }

        public void SwitchEnvironment(int index)
        {
            environments[CurrentEnvironment].SetActive(false);
            environments[index].SetActive(true);
            CurrentEnvironment = index;
            TooltipManager.HideTooltip();
        }

        public void LoadWordFillPuzzle(int index, UnityAction rewardAction)
        {
            Debug.Log("Loading wordfill puzzle");
            PuzzleManager.LoadWordFillPuzzle(index, rewardAction);
        }

        public void LoadRotatingLockPuzzle(int index, UnityAction rewardAction)
        {
            Debug.Log("Loading rotating lock puzzle");
            PuzzleManager.LoadRotatingLockPuzzle(index, rewardAction);
        }

        public void LoadImageGuessPuzzle(int index, UnityAction rewardAction)
        {
            Debug.Log("Loading image guess puzzle");
            PuzzleManager.LoadImageGuessPuzzle(index, rewardAction);
        }

        public void LoadSaveData(int lastEnvironment)
        {
            SwitchEnvironment(lastEnvironment);
        }
    }
}
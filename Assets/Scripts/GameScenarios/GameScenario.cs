using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WordHoarder.Gameplay;
using WordHoarder.Gameplay.World;
using WordHoarder.Managers.Static.Generic;
using WordHoarder.Managers.Static.UI;

namespace WordHoarder.GameScenarios
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
            InteractiveManager.LoadWordFillPuzzle(index, rewardAction);
        }

        public void LoadRotatingLockPuzzle(int index, UnityAction rewardAction)
        {
            Debug.Log("Loading rotating lock puzzle");
            InteractiveManager.LoadRotatingLockPuzzle(index, rewardAction);
        }

        public void LoadImageGuessPuzzle(int index, UnityAction rewardAction)
        {
            Debug.Log("Loading image guess puzzle");
            InteractiveManager.LoadImageGuessPuzzle(index, rewardAction);
        }

        public void LoadSaveData(int lastEnvironment)
        {
            SwitchEnvironment(lastEnvironment);
        }
    }
}
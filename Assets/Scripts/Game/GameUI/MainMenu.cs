using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Setup;
using WordHoarder.Utility;
using static WordHoarder.Utility.SaveUtility;

namespace WordHoarder.Gameplay.UI
{

    public class MainMenu : Menu
    {
        [SerializeField]
        private Animator titleAnimator;
        [SerializeField]
        private AnimationClip loadingFadeClip;
        [SerializeField]
        private TextMeshProUGUI titleTMP;
        [SerializeField]
        private GameObject buttonPanel;


        private string sourceTitleHalf = "WORD-";

        private void Awake()
        {
            Init();
        }


        public void Init()
        {
            menuLocalizationHelper = GetComponentInChildren<MainMenuLocalizationHelper>();
            Button[] buttons = GameObject.FindObjectsOfType<Button>(true);
            foreach (Button b in buttons)
                b.onClick.AddListener(() => SoundManager.PlaySound(SoundManager.Sound.Click));
            StartCoroutine(DisplayTitle());
        }

        private IEnumerator DisplayTitle()
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < sourceTitleHalf.Length; i++)
            {
                titleTMP.text = titleTMP.text + sourceTitleHalf[i];
                yield return new WaitForSeconds(0.3f);
            }

            titleAnimator.SetTrigger("DisplayTitleSecondHalf");

        }

        public void EnableButtonInteraction()
        {
            var buttons = buttonPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = true;
            }
        }

        public void StartGame()
        {
            AnimationEvent animEvent = new AnimationEvent();
            animEvent.time = loadingFadeClip.length - 0.01f;
            animEvent.functionName = "StartGameProcess";
            loadingFadeClip.AddEvent(animEvent);
            titleAnimator.SetBool("DisplayLoadingScreen", true);
        }

        private void StartGameProcess()
        {
            GameSetup.GetInstance().InitGame();
            LevelManager.LoadTutorial();
        }

        public void LoadGame(int index)
        {
            AnimationEvent animEvent = new AnimationEvent();
            animEvent.time = loadingFadeClip.length - 0.01f;
            animEvent.intParameter = index;
            animEvent.functionName = "LoadGameProcess";
            loadingFadeClip.AddEvent(animEvent);
            titleAnimator.SetBool("DisplayLoadingScreen", true);
        }

        private void LoadGameProcess(int index)
        {
            GameSetup.GetInstance().InitGame();
            LevelManager.LoadExistingGame(savesData[index]);
        }
    }
}

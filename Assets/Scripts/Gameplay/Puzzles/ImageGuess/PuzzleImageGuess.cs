using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WordHoarder.Managers.Instanced;
using WordHoarder.Managers.Static.Localization;
using WordHoarder.Managers.Static.UI;

namespace WordHoarder.Gameplay.Puzzles
{
    public class PuzzleImageGuess : MonoBehaviour
    {
        private class PuzzleInfo
        {
            public string imageName;
            public string optionA;
            public string optionB;
            public string optionC;

            public static PuzzleInfo CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<PuzzleInfo>(jsonString);
            }
        }

        private Image image;
        private Button[] buttons;
        private TextMeshProUGUI questionText;
        private Image backgroundPanel;

        private UnityAction rewardAction;

        public void InitPuzzle(TextAsset puzzle, UnityAction rewardAction)
        {
            this.rewardAction = rewardAction;
            buttons = GetComponentsInChildren<Button>();
            backgroundPanel = transform.GetChild(0).GetComponent<Image>();
            questionText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            image = transform.GetChild(2).GetComponent<Image>();
            PuzzleInfo puzzleInfo = PuzzleInfo.CreateFromJSON(puzzle.text);
            string[] optionTexts = new string[] { puzzleInfo.optionA, puzzleInfo.optionB, puzzleInfo.optionC };
            for (int i = 0; i < buttons.Length; i++)
            {
                int ii = i;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = optionTexts[i].ToUpper();
                if (optionTexts[i].ToLower() == puzzleInfo.imageName.ToLower())
                {
                    buttons[i].onClick.AddListener(() => SelectAnswer(ii, true));
                }
                else
                {
                    buttons[i].onClick.AddListener(() => SelectAnswer(ii, false));
                }
            }
            Sprite sprite = AssetsManager.GetSprite(puzzleInfo.imageName);
            image.sprite = sprite;
            image.SetNativeSize();
            RectTransform canvasRect = image.transform.parent.GetComponent<RectTransform>();
            if (canvasRect.rect.yMax - questionText.preferredHeight * 2 < image.rectTransform.rect.yMax)
            {
                float scaleRatio = image.rectTransform.sizeDelta.y / image.rectTransform.sizeDelta.x;
                float newY = canvasRect.rect.yMax - questionText.preferredHeight * 2;
                float newX = newY / scaleRatio;
                image.rectTransform.sizeDelta = new Vector2(newX, newY);
            }
            backgroundPanel.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
            LocalizationManager.onLanguageChanged += UpdateLocalization;
            UpdateLocalization();
        }

        private void SelectAnswer(int buttonIndex, bool isCorrect)
        {
            StartCoroutine(FlashButton(buttons[buttonIndex], isCorrect, 1f));
        }

        private IEnumerator FlashButton(Button button, bool success, float seconds)
        {
            button.interactable = false;
            var buttonColors = button.colors;
            if (!success)
            {
                buttonColors.disabledColor = Color.red;
                button.colors = buttonColors;
                yield return new WaitForSeconds(seconds);
                button.interactable = true;
            }
            else
            {
                buttonColors.disabledColor = Color.green;
                button.colors = buttonColors;
                yield return new WaitForSeconds(seconds);
                rewardAction.Invoke();
                button.interactable = true;
                InteractiveManager.ToggleInteraction();
            }
        }

        private void UpdateLocalization()
        {
            questionText.text = LocalizationManager.GetActiveLanguage().PuzzleImageGuessQuestion;
        }

        private void OnDisable()
        {
            LocalizationManager.onLanguageChanged -= UpdateLocalization;
        }
    }
}
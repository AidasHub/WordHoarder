using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using WordHoarder.Managers.Static.Localization;
using WordHoarder.Managers.Static.UI;

namespace WordHoarder.Gameplay.Puzzles
{
    public class PuzzleRotatingLock : MonoBehaviour
    {
        private class PuzzleInfo
        {
            public string word;
            public string descriptionEN;
            public string descriptionLT;

            public static PuzzleInfo CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<PuzzleInfo>(jsonString);
            }
        }

        [SerializeField]
        private GameObject lockPiecePrefab;
        private RectTransform canvas;

        private GameObject[] lockPieces;
        private RotatingLockLetter[] lockLetters;
        private int randomLetterCount = 5;

        private TextMeshProUGUI descriptionText;
        private Button puzzleUnlockButton;
        private UnityAction rewardAction;
        private PuzzleInfo puzzleInfo;

        private int paddingX = 200;
        private int paddingY = 100;

        public void InitPuzzle(TextAsset puzzle, UnityAction rewardAction)
        {
            this.rewardAction = rewardAction;
            canvas = GetComponent<RectTransform>();
            puzzleUnlockButton = gameObject.transform.GetChild(1).GetComponent<Button>();
            var lockBackground = gameObject.transform.GetChild(0).GetComponent<RectTransform>();
            descriptionText = gameObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            puzzleInfo = PuzzleInfo.CreateFromJSON(puzzle.text);
            SetupLockPieces(canvas, puzzleInfo.word);


            // Adjusting created lock pieces
            var screenMiddleDistance = canvas.rect.size.x / 2;
            var sumButtonLength = lockPieces[0].GetComponent<RectTransform>().rect.width * lockPieces.Length;
            var sumButtonMiddleDistance = sumButtonLength / 2;

            var offset = screenMiddleDistance - sumButtonMiddleDistance;

            for (int i = 0; i < lockPieces.Length; i++)
            {
                RectTransform pieceRect = lockPieces[i].GetComponent<RectTransform>();
                pieceRect.anchoredPosition = new Vector2(pieceRect.anchoredPosition.x + offset, pieceRect.anchoredPosition.y);
            }

            // Unlock button
            var lockPieceButtonHeight = lockPieces[0].GetComponent<RectTransform>().rect.height + lockPieces[0].GetComponentInChildren<RectTransform>().rect.height;
            var puzzleUnlockButtonRect = puzzleUnlockButton.gameObject.GetComponent<RectTransform>();
            puzzleUnlockButtonRect.anchoredPosition = new Vector2(0, (0 - lockPieceButtonHeight));
            puzzleUnlockButtonRect.sizeDelta = new Vector2(sumButtonLength / 3, lockPieceButtonHeight / 3);
            puzzleUnlockButton.onClick.AddListener(AttemptUnlock);

            // Background
            var backgroundHeight = lockPieceButtonHeight + paddingY;
            lockBackground.sizeDelta = new Vector2(sumButtonLength + paddingX, backgroundHeight);

            // Description text
            LocalizationManager.onLanguageChanged += UpdateLocalization;
            UpdateLocalization();
            var textRect = descriptionText.GetComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(canvas.rect.size.x, textRect.sizeDelta.y);
            textRect.anchoredPosition = new Vector2(0, (0 - canvas.rect.y - descriptionText.preferredHeight));
        }

        private void SetupLockPieces(RectTransform canvas, string word)
        {
            var canvasLength = canvas.rect.size.x;
            float x = 0;
            float y = 0;
            float z = 0;

            lockPieces = new GameObject[word.Length];
            lockLetters = new RotatingLockLetter[word.Length];
            for (int i = 0; i < lockPieces.Length; i++)
            {
                // Creating and positioning a lock piece
                var go = Instantiate(lockPiecePrefab, transform);
                go.name = "Piece " + i;
                var goRect = go.GetComponent<RectTransform>();
                x = 0 - (canvasLength / 2) + goRect.rect.width / 2 + i * goRect.rect.width;

                goRect.anchoredPosition = new Vector3(x, y, z);

                // Adding appropriate character to a lock piece along with random characters
                TextMeshProUGUI goText = go.GetComponentInChildren<TextMeshProUGUI>();
                lockLetters[i] = new RotatingLockLetter(char.ToUpper(word[i]), randomLetterCount, goText);

                // Setting up buttons for cycling through characters
                var buttons = go.GetComponentsInChildren<Button>();
                int ii = i;
                buttons[0].onClick.AddListener(() => ChangeActiveLetter(ii, true));
                buttons[1].onClick.AddListener(() => ChangeActiveLetter(ii, false));

                lockPieces[i] = go;
            }
        }


        public void ChangeActiveLetter(int buttonIndex, bool isNext)
        {
            lockLetters[buttonIndex].ChangeActiveLetter(isNext);
        }

        private void AttemptUnlock()
        {
            bool allLettersAreCorrect = true;
            for (int i = 0; i < lockLetters.Length; i++)
            {
                if (!lockLetters[i].ActiveLetterIsCorrect())
                {
                    allLettersAreCorrect = false;
                    break;
                }
            }

            StartCoroutine(FlashButton(allLettersAreCorrect, 1f));
        }

        private IEnumerator FlashButton(bool success, float seconds)
        {
            puzzleUnlockButton.interactable = false;
            var buttonColors = puzzleUnlockButton.colors;
            if (!success)
            {
                buttonColors.disabledColor = Color.red;
                puzzleUnlockButton.colors = buttonColors;
                yield return new WaitForSeconds(seconds);
                puzzleUnlockButton.interactable = true;
            }
            else
            {
                buttonColors.disabledColor = Color.green;
                puzzleUnlockButton.colors = buttonColors;
                yield return new WaitForSeconds(seconds);
                rewardAction.Invoke();
                puzzleUnlockButton.interactable = true;
                InteractiveManager.ToggleInteraction();
                Close();
            }
        }

        private void UpdateLocalization()
        {
            if (LocalizationManager.CurrentlyActiveLanguage == LocalizationManager.ActiveLanguage.EN)
                descriptionText.text = puzzleInfo.descriptionEN;
            if (LocalizationManager.CurrentlyActiveLanguage == LocalizationManager.ActiveLanguage.LT)
                descriptionText.text = puzzleInfo.descriptionLT;
            puzzleUnlockButton.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationManager.GetActiveLanguage().MiscUnlock;
        }

        private void OnDisable()
        {
            LocalizationManager.onLanguageChanged -= UpdateLocalization;
        }

        public void Close()
        {
            for (int i = 0; i < lockPieces.Length; i++)
            {
                puzzleUnlockButton.onClick.RemoveAllListeners();
                Destroy(lockPieces[i].gameObject);
            }
        }
    }
}
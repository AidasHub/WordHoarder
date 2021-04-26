using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WordHoarder.Gameplay.Puzzles;
using WordHoarder.Localization;
using WordHoarder.Managers.Static.Gameplay;
using WordHoarder.Managers.Static.UI;

namespace WordHoarder.Gameplay.World
{
    public class WorldInteractable : MonoBehaviour, IDropHandler
    {
        [SerializeField]
        private string expectedWord;
        [SerializeField]
        private UnityEvent actionEvent;
        private Image image;
        private bool isComplete;


        private void Awake()
        {
            isComplete = false;
            image = GetComponent<Image>();
            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            if(collider != null)
                collider.size = new Vector2(image.rectTransform.rect.width, image.rectTransform.rect.height);
            if (actionEvent.GetPersistentEventCount() == 0)
                actionEvent.AddListener(RevealWord);
        }

        private void OnMouseOver()
        {
            if(!InteractiveManager.InteractivePanelOpen && !InventoryManager.IsOpen && !GameManager.GamePaused)
            TooltipManager.DrawTooltip(LocalizationManager.GetActiveLanguage().ReverseWordTooltip);
        }

        private void OnMouseExit()
        {
            TooltipManager.HideTooltip();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var droppedGO = eventData.pointerDrag.gameObject;
                var actualWord = droppedGO.GetComponent<TextMeshProUGUI>().text;

                if (expectedWord.ToLower() == actualWord.ToLower())
                {
                    InventoryManager.RemoveWord(expectedWord);
                    Destroy(droppedGO);
                    if (actionEvent != null)
                        actionEvent.Invoke();
                }
                else
                {
                    FlashWordSlot();
                    droppedGO.GetComponent<InventoryWord>().ResetPosition();
                }
            }
        }

        private void FlashWordSlot()
        {
            StartCoroutine(WordFlashing(GetComponent<Image>()));
        }

        private IEnumerator WordFlashing(Image image)
        {
            Color oldColor = image.color;
            image.color = Color.red;
            yield return new WaitForSeconds(1f);
            image.color = oldColor;
        }

        public void RevealWord()
        {
            StartCoroutine(RevealWordAnimation(1f));
        }

        private IEnumerator RevealWordAnimation(float seconds)
        {
            Color color = image.color;
            float colorStep = 1f / seconds;
            float timeStep = 0.01f;
            while(image.color.a > 0)
            {
                color.a -= colorStep * timeStep;
                image.color = color;
                yield return new WaitForSeconds(timeStep);
            }
            isComplete = true;
            gameObject.SetActive(false);
        }

        public Tuple<string, bool> PrepareSaveData()
        {
            string interactableName = gameObject.name;
            Tuple<string, bool> saveData = new Tuple<string, bool>(interactableName, isComplete);
            return saveData;
        }

        public void LoadSaveData(bool isComplete)
        {
            if (isComplete)
                gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            TooltipManager.HideTooltip();
        }
    }
}
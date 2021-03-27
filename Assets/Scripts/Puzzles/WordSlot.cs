using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WordSlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private string expectedWord;
    [SerializeField]
    private int expectedWordIndex;
    private PuzzleWordFill puzzle;

    private void Awake()
    {
        puzzle = GetComponentInParent<PuzzleWordFill>();
    }


    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            var droppedGO = eventData.pointerDrag.gameObject;
            var actualWord = droppedGO.GetComponent<TextMeshProUGUI>().text;

            if (expectedWord == actualWord)
            {
                this.gameObject.SetActive(false);
                puzzle.UpdatePuzzle(actualWord, expectedWordIndex);
                Destroy(droppedGO);
            }
            else
            {
                FlashWordSlot();
                droppedGO.GetComponent<InventoryWord>().ResetPosition();
            }

        }
    }

    public void SetWord(string word, int index)
    {
        this.expectedWord = word;
        this.expectedWordIndex = index;
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
}

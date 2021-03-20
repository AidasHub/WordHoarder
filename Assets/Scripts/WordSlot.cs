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
    private WordPuzzle puzzle;

    private void Awake()
    {
        puzzle = GetComponentInParent<WordPuzzle>();
    }


    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            var droppedGO = eventData.pointerDrag.gameObject;
            if (droppedGO.GetComponent<TextMeshProUGUI>().text == expectedWord)
            {
                droppedGO.SetActive(false);
                this.gameObject.SetActive(false);
                puzzle.UpdatePuzzle(droppedGO.GetComponent<TextMeshProUGUI>().text);
            }
            else
            {
                FlashWordSlot();
                droppedGO.GetComponent<Word_Inventory>().ResetPosition();
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
}

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

    private void Awake()
    {
        
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
            }
            else
            {
                FlashWordSlot();
                droppedGO.GetComponent<InteractableWord>().ResetPosition();
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

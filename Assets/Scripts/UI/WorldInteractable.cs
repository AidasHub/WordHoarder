using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldInteractable : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private string expectedWord;
    [SerializeField]
    private UnityEvent actionEvent;
    private Button button;


    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            var droppedGO = eventData.pointerDrag.gameObject;
            var actualWord = droppedGO.GetComponent<TextMeshProUGUI>().text;

            if (expectedWord == actualWord)
            {
                Debug.Log("Before:" + InventoryManager.GetWords().Count);
                InventoryManager.RemoveWord(expectedWord);
                Debug.Log("After:" + InventoryManager.GetWords().Count);
                Destroy(droppedGO);
                //button.onClick.Invoke();
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
}

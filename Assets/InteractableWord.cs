using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class InteractableWord : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rect;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector3 startingPosition;
    private bool startDrag;

    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (startDrag)
        {
            startingPosition = rect.position;
            startDrag = false;
        }
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        startDrag = true;
        ResetPosition();
    }

    public void ResetPosition()
    {
        rect.position = startingPosition;
    }
}

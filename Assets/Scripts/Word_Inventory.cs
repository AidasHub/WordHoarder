using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Word_Inventory : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rect;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector3 startingPosition;
    private bool startDrag;
    private Transform preDragParent;
    private int preDragSiblingIndex;

    private void Awake()
    {
        canvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startDrag = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (startDrag)
        {
            preDragParent = transform.parent;
            preDragSiblingIndex = transform.GetSiblingIndex();
            transform.SetParent(canvas.transform);
            startingPosition = rect.position;
            startDrag = false;
        }
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(preDragParent);
        transform.SetSiblingIndex(preDragSiblingIndex);
        preDragParent = null;
        preDragSiblingIndex = -1;
        canvasGroup.blocksRaycasts = true;
        startDrag = true;
        ResetPosition();
    }

    public void ResetPosition()
    {
        rect.position = startingPosition;
    }
}

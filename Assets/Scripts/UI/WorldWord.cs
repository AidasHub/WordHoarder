using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D)), RequireComponent(typeof(LineRenderer))]
public class WorldWord : MonoBehaviour
{
    [SerializeField]
    private string wordText;
    private PolygonCollider2D wordCollider;
    private LineRenderer lineRenderer;

    private void Start()
    {
        wordCollider = GetComponent<PolygonCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        CalculateObjectOutline();
    }

    void OnMouseOver()
    {
        if(enabled)
            DrawObjectOutline();
        //MouseManager.getInstance().ActivateWordTooltip(Input.mousePosition, wordText);
    }


    void OnMouseExit()
    {
        if(enabled)
            EraseObjectOutline();
        //MouseManager.getInstance().HideWordTooltip();
    }

    private void OnMouseDown()
    {
        if(enabled)
        {
            wordCollider.enabled = false;
            EraseObjectOutline();
            InventoryManager.getInstance().AddWord(wordText);
        }
    }

    private void CalculateObjectOutline()
    {
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = wordCollider.points.Length + 1;
        for(int i = 0; i < wordCollider.points.Length; i++)
        {
            Vector3 newPosition = new Vector3(wordCollider.points[i].x, wordCollider.points[i].y, -1);
            lineRenderer.SetPosition(i, newPosition);
        }
        Vector3 lastLinePosition = new Vector3(wordCollider.points[0].x, wordCollider.points[0].y, -1);
        lineRenderer.SetPosition(wordCollider.points.Length, lastLinePosition);
    }

    private void DrawObjectOutline()
    {
        // To-do: move it to events
        float referenceRatio = (float)16 / 9;
        float referenceScale = 1f;
        float currentRatio = (float)Screen.width / Screen.height;
        float newScale = referenceScale * referenceRatio / currentRatio;

        Vector3 newScaleVector = new Vector3(transform.localScale.x, newScale, transform.localScale.z);
        transform.localScale = newScaleVector;
        if (!GameManager.GamePaused)
            lineRenderer.enabled = true;
    }

    private void EraseObjectOutline()
    {
        lineRenderer.enabled = false;
    }

    public Tuple<string, bool> PrepareSaveData()
    {
        string word = wordText;
        bool collected = gameObject.activeInHierarchy;
        Tuple<string, bool> saveData = new Tuple<string, bool>(word, collected);
        return saveData;
    }
}

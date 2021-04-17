using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D)), RequireComponent(typeof(LineRenderer))]
public class WorldWord : MonoBehaviour
{
    [SerializeField]
    private string wordText;
    private PolygonCollider2D wordCollider;
    private LineRenderer lineRenderer;

    private TextMeshProUGUI pickupText;

    private void Awake()
    {
        wordCollider = GetComponent<PolygonCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        CalculateObjectOutline();
    }

    void OnMouseOver()
    {
        if(enabled && !InteractiveManager.InteractivePanelOpen && !GameManager.GamePaused)
            DrawObjectOutline();
    }


    void OnMouseExit()
    {
        EraseObjectOutline();
    }

    private void OnMouseDown()
    {
        if(enabled && !InteractiveManager.InteractivePanelOpen && !GameManager.GamePaused)
        {
            wordCollider.enabled = false;
            EraseObjectOutline();
            InventoryManager.AddWord(wordText);
            DisplayPickupText();
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
        string word = gameObject.name;
        if (wordCollider == null)
            Awake();
        bool isCollected = !wordCollider.enabled;
        Tuple<string, bool> saveData = new Tuple<string, bool>(word, isCollected);
        return saveData;
    }

    public void LoadSaveData(bool isCollected)
    {
        if (wordCollider == null)
            Awake();
        wordCollider.enabled = !isCollected;
    }

    private void DisplayPickupText()
    {
        var pickupTextObject = new GameObject(wordText);
        pickupTextObject.transform.SetParent(gameObject.transform);

        // Text properties
        pickupText = pickupTextObject.AddComponent<TextMeshProUGUI>();
        pickupText.alignment = TextAlignmentOptions.Midline;
        pickupText.text = wordText.ToUpper();
        pickupText.color = Color.green;
        pickupText.fontSize = 0;
        pickupText.outlineColor = Color.black;
        pickupText.outlineWidth = 0.2f;
        pickupText.enableWordWrapping = false;

        // Position
        var pickupTextRect = pickupTextObject.GetComponent<RectTransform>();
        pickupTextRect.localScale = new Vector3(1, 1, 1);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out localPoint);
        pickupTextRect.localPosition = localPoint;
        StartCoroutine(PlayPickupTextAnimation(pickupText, 0.3f));
    }

    private IEnumerator PlayPickupTextAnimation(TextMeshProUGUI tmp, float seconds)
    {
        float debugSeconds = Time.realtimeSinceStartup;
        var increaseSizeSeconds = seconds * 4 / 5;
        var reduceSizeSeconds = seconds / 5;
        float sizeStep = 72 / seconds;

        float currentTime = 0f;
        while (currentTime < increaseSizeSeconds)
        {
            var elapsedTime = Time.deltaTime;
            tmp.fontSize += sizeStep * elapsedTime;
            currentTime += elapsedTime;
            yield return null;
        }

        currentTime = 0f;
        while(currentTime < reduceSizeSeconds)
        {
            var elapsedTime = Time.deltaTime;
            tmp.fontSize -= sizeStep * elapsedTime;
            currentTime += elapsedTime;
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        Destroy(tmp.gameObject);
    }

    private void OnDisable()
    {
        if(pickupText != null)
        {
            Destroy(pickupText.gameObject);
        }
    }
}

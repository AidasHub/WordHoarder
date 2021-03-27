using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WorldWord : MonoBehaviour
{
    [SerializeField]
    private string wordText;
    private BoxCollider2D wordCollider;

    private void Start()
    {
        wordCollider = GetComponent<BoxCollider2D>();
    }

    void OnMouseOver()
    {
        MouseManager.getInstance().ActivateWordTooltip(Input.mousePosition, wordText);
    }

    void OnMouseExit()
    {
        MouseManager.getInstance().HideWordTooltip();
    }

    private void OnMouseDown()
    {
        wordCollider.enabled = false;
        InventoryManager.getInstance().AddWord(wordText);
    }
}

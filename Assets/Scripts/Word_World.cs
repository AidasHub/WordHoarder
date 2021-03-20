using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word_World : MonoBehaviour
{
    [SerializeField]
    private string wordText;

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
        this.gameObject.SetActive(false);
    }
}

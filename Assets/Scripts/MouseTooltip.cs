using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseTooltip : MonoBehaviour
{
    
    private GameObject tooltipTextGameObject;
    private TextMeshProUGUI tooltipText;

    private void Awake()
    {
        MouseManager.getInstance().setTooltip(this);
    }

    private void Start()
    {
        tooltipTextGameObject = transform.GetChild(0).GetChild(0).gameObject;
        tooltipText = tooltipTextGameObject.GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public void SetTooltip(Vector3 newPosition, string text)
    {
        tooltipTextGameObject.transform.position = newPosition;
        tooltipText.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseTooltipPrefab;

    private GameObject mouseTooltipGameObject;
    private MouseTooltip mouseTooltip;

    private static MouseManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Init();
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        mouseTooltipGameObject = Instantiate(mouseTooltipPrefab);
        mouseTooltip = mouseTooltipGameObject.GetComponent<MouseTooltip>();
    }

    public static MouseManager getInstance()
    {
        return instance;
    }

    public void setTooltip(MouseTooltip mouseTooltip)
    {
        this.mouseTooltip = mouseTooltip;
    }

    public void ActivateWordTooltip(Vector3 wordPosition, string text)
    {
        mouseTooltip.gameObject.SetActive(true);
        mouseTooltip.SetTooltip(wordPosition, text);
    }

    public void HideWordTooltip()
    {
        mouseTooltip.gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST : MonoBehaviour
{
    public GameObject prefab;
    public RectTransform canvas;
    int buttonCount = 7;


    void Start()
    {
        var canvasLength = canvas.rect.size.x;

        var x = 0f;
        var y = 0f;
        var z = 0;
        List<RectTransform> buttonList = new List<RectTransform>();
        for (int i = 0; i < buttonCount; i++)
        {
            var go = Instantiate(prefab, canvas.gameObject.transform);
            var goRect = go.GetComponent<RectTransform>();
            x = 0 - (canvasLength / 2) + goRect.rect.width / 2 + i * goRect.rect.width;

            goRect.anchoredPosition = new Vector3(x, y, z);
            buttonList.Add(goRect);
        }

        var screenMiddle = canvas.rect.size.x / 2;
        var sumButtonLength = buttonList[0].rect.width * buttonCount;
        var sumButtonMiddle = sumButtonLength / 2;

        var offset = screenMiddle - sumButtonMiddle;


        foreach(RectTransform r in buttonList)
        {
            r.anchoredPosition = new Vector2(r.anchoredPosition.x + offset, r.anchoredPosition.y);
        }
    }
}

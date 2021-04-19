using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WordHoarder.Managers.Instanced;

namespace WordHoarder.Managers.Static.UI
{
    public static class TooltipManager
    {
        private static GameObject tooltip;
        private static TextMeshProUGUI tooltipText;
        private static RectTransform tooltipBackground;

        private static void CreateTooltip()
        {
            GameObject tooltipPrefab = AssetsManager.ManagerPrefabs.Tooltip;
            tooltip = UIManager.AddToCanvas(tooltipPrefab);
            tooltipBackground = tooltip.transform.GetChild(0).GetComponent<RectTransform>();
            tooltipText = tooltip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        public static void HideTooltip()
        {
            if (tooltip != null)
                tooltip.SetActive(false);
        }

        public static void DrawTooltip(string text)
        {
            if (tooltip == null)
            {
                CreateTooltip();
            }
            else
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(tooltip.transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out localPoint);
                tooltip.GetComponent<RectTransform>().localPosition = localPoint;

                tooltipText.text = text;
                var backgroundW = tooltipText.preferredWidth;
                var backgroundH = tooltipText.preferredHeight;

                tooltipBackground.sizeDelta = new Vector2(backgroundW, backgroundH);

                tooltip.SetActive(true);
            }
        }
    }
}
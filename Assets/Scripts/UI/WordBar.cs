using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI sliderText;
    private float collectionPercentage;

    private static WordBar instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void Init()
    { 
        GameManager.onWordCollected.AddListener(UpdateWordsCollected);
        slider.maxValue = GameManager.TotalWords;
        UpdateWordsCollected();
        LocalizationManager.onLanguageChanged += UpdateWordsCollected;
    }

    public void UpdateWordsCollected()
    {
        slider.value = GameManager.CollectedWords;
        //sliderText.text = GameManager.CollectedWords + "/" + GameManager.TotalWords + " words collected";
        collectionPercentage = (100 * (float)GameManager.CollectedWords / GameManager.TotalWords);
        sliderText.text = string.Format("{0:0.0}% {1}", collectionPercentage, LocalizationManager.GetActiveLanguage().WordsCollected);
    }

    public static void ShowWordBar()
    {
        instance.gameObject.SetActive(true);
    }

    public static void HideWordBar()
    {
        instance.gameObject.SetActive(false);
    }
}

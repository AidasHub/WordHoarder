using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Animator titleAnimator;
    [SerializeField]
    private TextMeshProUGUI titleTMP;
    [SerializeField]
    private GameObject buttonPanel;

    private string sourceTitleHalf = "WORD-";

    private void Awake()
    {
        Init();
    }


    public void Init()
    {
        StartCoroutine(DisplayTitle());
    }

    private IEnumerator DisplayTitle()
    {
        yield return new WaitForSeconds(1f);

        for(int i = 0; i < sourceTitleHalf.Length; i++)
        {
            titleTMP.text = titleTMP.text + sourceTitleHalf[i];
            yield return new WaitForSeconds(0.3f);
        }

        titleAnimator.SetTrigger("DisplayTitleSecondHalf");

    }

    public void EnableButtonInteraction()
    {
        var buttons = buttonPanel.GetComponentsInChildren<Button>();
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScenario : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorialPanel;
    [SerializeField]
    private GameObject escapeMenu;


    private UIManager _UIManager;

    private void Awake()
    {
        _UIManager = UIManager.getInstance();
        Init();
    }

    private void Init()
    {
        _UIManager.AddToCanvas(tutorialPanel);
        _UIManager.AddToCanvas(escapeMenu);
    }
}

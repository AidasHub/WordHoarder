using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;

public class EndScenario : MonoBehaviour
{
    public void Awake()
    {
        UIManager.Destroy();
    }

    public void ReturnToMainMenu()
    {
        GameSetup.GetInstance().ReturnToMainMenu();
    }
}

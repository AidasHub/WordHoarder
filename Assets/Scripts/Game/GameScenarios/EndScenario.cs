using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WordHoarder.Managers.Static.UI;
using WordHoarder.Setup;

namespace WordHoarder.Gameplay.GameScenarios
{
    public class EndScenario : MonoBehaviour
    {
        public void ReturnToMainMenu()
        {
            GameSetup.GetInstance().ReturnToMainMenu();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScenario : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> environments;
    private int currentEnvironment;

    public void SwitchEnvironment(int index)
    {
        environments[currentEnvironment].SetActive(false);
        environments[index].SetActive(true);
        currentEnvironment = index;
    }

    public void LoadPuzzle(int index)
    {
        InteractiveManager.getInstance().LoadWordFillPuzzle(index);
    }
}

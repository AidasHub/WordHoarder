using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScenario : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> environments;
    public int CurrentEnvironment { get; private set; }

    public void SwitchEnvironment(int index)
    {
        environments[CurrentEnvironment].SetActive(false);
        environments[index].SetActive(true);
        CurrentEnvironment = index;
    }

    public void LoadSaveData(int lastEnvironment)
    {
        SwitchEnvironment(lastEnvironment);
    }
}

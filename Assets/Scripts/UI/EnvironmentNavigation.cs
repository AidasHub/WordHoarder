using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentNavigation : MonoBehaviour
{
    [SerializeField]
    private Button lockedButton;
    [SerializeField]
    private Button unlockedButton;
    [SerializeField]
    private bool isLocked;

    public void Awake()
    {
        SetupEnvironment();
    }

    public void SetupEnvironment()
    {
        if (isLocked)
        {
            lockedButton.gameObject.SetActive(true);
            unlockedButton.interactable = false;
        }
        else
        {
            if (lockedButton != null)
                lockedButton.gameObject.SetActive(false);
            unlockedButton.gameObject.SetActive(true);
            unlockedButton.interactable = true;
        }
    }

    public void UnlockEnvironment()
    {
        if(lockedButton != null)
            lockedButton.gameObject.SetActive(false);
        unlockedButton.interactable = true;
        isLocked = false;
    }

    public void LoadWordFillPuzzle(int index)
    {
        InteractiveManager.getInstance().LoadWordFillPuzzle(index, UnlockEnvironment);
    }

    public Tuple<string, bool> PrepareSaveData()
    {
        string navigationTo = gameObject.name;
        Tuple<string, bool> saveData = new Tuple<string, bool>(navigationTo, isLocked);
        return saveData;
    }

    public void LoadSaveData(bool isLocked)
    {
        if (!isLocked)
            UnlockEnvironment();
    }
}

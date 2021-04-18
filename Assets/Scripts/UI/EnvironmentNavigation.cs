using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnvironmentNavigation : MonoBehaviour
{
    private enum EnvironmentDestination
    {
        ToLivingRoom,
    }

    [SerializeField]
    private Button lockedButton;
    [SerializeField]
    private Button unlockedButton;
    [SerializeField]
    private bool isLocked;
    [SerializeField]
    EnvironmentDestination destination;

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
        Debug.Log("Loading wordfill puzzle");
        InteractiveManager.LoadWordFillPuzzle(index, UnlockEnvironment);
    }

    public void LoadRotatingLockPuzzle(int index)
    {
        Debug.Log("Loading rotating lock puzzle");
        InteractiveManager.LoadRotatingLockPuzzle(index, UnlockEnvironment);
    }

    public void LoadImageGuessPuzzle(int index)
    {
        Debug.Log("Loading image guess puzzle");
        InteractiveManager.LoadImageGuessPuzzle(index, UnlockEnvironment);
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

    private void OnMouseOver()
    {
        if (lockedButton.gameObject.activeInHierarchy && !InteractiveManager.InteractivePanelOpen &&!GameManager.GamePaused)
        {
            string text = GetEnvironmentDestinationText(destination);
            TooltipManager.DrawTooltip(text);
        }
    }

    

    private void OnMouseExit()
    {
        TooltipManager.HideTooltip();
    }

    private string GetEnvironmentDestinationText(EnvironmentDestination destination)
    {
        var language = LocalizationManager.GetActiveLanguage();
        switch (destination)
        {
            default:
                return "Error";
            case EnvironmentDestination.ToLivingRoom:
                return language.EnvironmentToLivingRoom;
        }
    }
}

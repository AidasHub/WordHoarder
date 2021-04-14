using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLocalization : MonoBehaviour
{
    [Header("Main Menu Panel Text")]
    [SerializeField]
    Text menuStart;
    [SerializeField]
    Text menuLoad;
    [SerializeField]
    Text menuOptions;
    [SerializeField]
    Text menuQuit;

    [Header("Loading Panel Text")]
    [SerializeField]
    List<Text> loadSlots;
    [SerializeField]
    Text loadBack;

    [Header("Options Menu Panel Text")]
    [SerializeField]
    Text optionsGraphics;
    [SerializeField]
    Text optionsAudio;
    [SerializeField]
    Text optionsBack;

    [Header("Graphics Menu Panel Text")]
    [SerializeField]
    Text graphicsResolution;
    [SerializeField]
    Text graphicsFullScreen;
    [SerializeField]
    Text graphicsBack;

    public void Start()
    {
        UpdateLanguage();
    }

    public void SetLanguage(int index)
    {
        LocalizationManager.SetActiveLanguage(index);
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        var language = LocalizationManager.GetActiveLanguage();

        menuStart.text = language.MenuStart.ToUpper();
        menuLoad.text = language.MenuLoad.ToUpper();
        menuOptions.text = language.MenuOptions.ToUpper();
        menuQuit.text = language.MenuQuit.ToUpper();

        loadBack.text = language.MiscBack.ToUpper();

        optionsGraphics.text = language.OptionsGraphics.ToUpper();
        optionsAudio.text = language.OptionsAudio.ToUpper();
        optionsBack.text = language.MiscBack.ToUpper();

        graphicsResolution.text = language.GraphicsResolution.ToUpper();
        graphicsFullScreen.text = language.GraphicsFullScreen.ToUpper();
        graphicsBack.text = language.MiscBack.ToUpper();
    }

    public void UpdateLanguageForLoadSlots(string[] slotInfo)
    {
        var language = LocalizationManager.GetActiveLanguage();
        for(int i = 0; i < loadSlots.Count; i++)
        {
            if (slotInfo[i] == null)
            {
                loadSlots[i].text = language.LoadSlotEmpty.ToUpper();
            }
            else
            {
                loadSlots[i].text = slotInfo[i] + " " + language.LoadSlotUsed.ToUpper();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLocalization : MonoBehaviour
{
    [Header("Main Menu Panel Text")]
    [SerializeField]
    Text MenuStart;
    [SerializeField]
    Text MenuLoad;
    [SerializeField]
    Text MenuOptions;
    [SerializeField]
    Text MenuQuit;

    [Header("Options Menu Panel Text")]
    [SerializeField]
    Text OptionsGraphics;
    [SerializeField]
    Text OptionsAudio;
    [SerializeField]
    Text OptionsBack;

    public void SetLanguage(int index)
    {
        LocalizationManager.SetActiveLanguage(index);
        UpdateLanguage();
    }

    public void UpdateLanguage()
    {
        var language = LocalizationManager.GetActiveLanguage();

        MenuStart.text = language.MenuStart.ToUpper();
        MenuLoad.text = language.MenuLoad.ToUpper();
        MenuOptions.text = language.MenuOptions.ToUpper();
        MenuQuit.text = language.MenuQuit.ToUpper();

        OptionsGraphics.text = language.OptionsGraphics.ToUpper();
        OptionsAudio.text = language.OptionsAudio.ToUpper();
        OptionsBack.text = language.MiscBack.ToUpper();
    }
}

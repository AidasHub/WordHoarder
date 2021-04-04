using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuLocalization : MonoBehaviour
{
    [Header("Main Menu Panel Text")]
    [SerializeField]
    Text pauseLabel;
    [SerializeField]
    Text pauseResume;
    [SerializeField]
    Text pauseSave;
    [SerializeField]
    Text pauseOptions;
    [SerializeField]
    Text pauseQuit;

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

    private void Start()
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

        pauseLabel.text = language.PauseLabel.ToUpper();
        pauseResume.text = language.PauseResume.ToUpper();
        pauseSave.text = language.PauseSave.ToUpper();
        pauseOptions.text = language.MenuOptions.ToUpper();
        pauseQuit.text = language.PauseQuit.ToUpper();

        optionsGraphics.text = language.OptionsGraphics.ToUpper();
        optionsAudio.text = language.OptionsAudio.ToUpper();
        optionsBack.text = language.MiscBack.ToUpper();

        graphicsResolution.text = language.GraphicsResolution.ToUpper();
        graphicsFullScreen.text = language.GraphicsFullScreen.ToUpper();
        graphicsBack.text = language.MiscBack.ToUpper();
    }
}

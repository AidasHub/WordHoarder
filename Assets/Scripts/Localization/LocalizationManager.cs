using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class LocalizationManager
{
    public enum ActiveLanguage
    {
        EN = 0,
        LT = 1
    }

    [Serializable]
    public class _LocalizationInfo
    {
        // Main Menu Options
        public string MenuStart;
        public string MenuLoad;
        public string MenuOptions;
        public string MenuQuit;

        // Save/Load Menu Options
        public string LoadSlotEmpty;
        public string LoadSlotUsed;
        public string LoadSlotOverwrite;
        public string LoadSaveSuccessful;
        public string LoadSaveFailed;

        // Options Menu Options
        public string OptionsGraphics;
        public string OptionsAudio;

        // Graphics Menu Options
        public string GraphicsResolution;
        public string GraphicsFullScreen;

        // Pause Menu Options
        public string PauseLabel;
        public string PauseResume;
        public string PauseSave;
        public string PauseOptions;
        public string PauseQuit;

        // Miscellaneous Text
        public string MiscContinue;
        public string MiscBack;
        public string MiscLaunch;
        public string MiscYes;
        public string MiscNo;
        public string MiscOK;

        public string[] TutorialSteps;

        public static _LocalizationInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<_LocalizationInfo>(jsonString);
        }
    }

    private static List<_LocalizationInfo> languageInfoList = new List<_LocalizationInfo>();

    private static ActiveLanguage activeLanguage = ActiveLanguage.EN;

    public delegate void NotifyLanguageChange();
    public static event NotifyLanguageChange onLanguageChanged;

    public static void Init()
    {
        TextAsset[] localizationFiles = AssetsManager.Localization.Languages;
        for(int i = 0; i < localizationFiles.Length; i++)
        {
            var info = _LocalizationInfo.CreateFromJSON(localizationFiles[i].text);
            languageInfoList.Add(info);
        }
        activeLanguage = 0;
    }

    public static void SetActiveLanguage(int index)
    {
        activeLanguage = (ActiveLanguage)index;
        if (onLanguageChanged != null)
            onLanguageChanged.Invoke();
    }

    public static _LocalizationInfo GetActiveLanguage()
    {
        int activeLanguageInt = (int)activeLanguage;
        return languageInfoList[activeLanguageInt];
    }
}

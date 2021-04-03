using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class LocalizationManager : MonoBehaviour
{
    public enum ActiveLanguage
    {
        EN = 0,
        LT = 1
    }

    [Serializable]
    public class _LocalizationInfo
    {
        public string MenuStart;
        public string MenuLoad;
        public string MenuOptions;
        public string MenuQuit;

        public string OptionsGraphics;
        public string OptionsAudio;

        public string MiscContinue;
        public string MiscBack;

        public string[] TutorialSteps;

        public static _LocalizationInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<_LocalizationInfo>(jsonString);
        }
    }



    [SerializeField]
    List<TextAsset> localizationFiles;

    private static List<_LocalizationInfo> languageInfoList = new List<_LocalizationInfo>();

    private static ActiveLanguage activeLanguage;

    private static LocalizationManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public static LocalizationManager getInstance()
    {
        return instance;
    }

    private void Init()
    {
        for(int i = 0; i < localizationFiles.Count; i++)
        {
            var info = _LocalizationInfo.CreateFromJSON(localizationFiles[i].text);
            languageInfoList.Add(info);
        }
        activeLanguage = 0;
    }

    public static void SetActiveLanguage(int index)
    {
        activeLanguage = (ActiveLanguage)index;
    }

    public static _LocalizationInfo GetActiveLanguage()
    {
        int activeLanguageInt = (int)activeLanguage;
        return languageInfoList[activeLanguageInt];
    }
}

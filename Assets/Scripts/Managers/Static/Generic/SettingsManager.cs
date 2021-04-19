using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WordHoarder.Managers.Static.Generic
{
    public static class SettingsManager
    {
        public static Resolution[] GetResolutions()
        {
            return Screen.resolutions;
        }

        public static void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, Screen.fullScreenMode);
        }

        public static bool GetFullScreenMode()
        {
            return Screen.fullScreen;
        }

        public static void SetFullScreenMode(bool fullScreen)
        {
            Screen.fullScreen = fullScreen;
        }
    }
}
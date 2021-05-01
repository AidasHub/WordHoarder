using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WordHoarder.Utility
{
    public static class SettingsManager
    {

        private static float audioVolume = 1f;
        private static bool audioEnabled = true;

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

        public static float GetAudioVolume()
        {
            return audioVolume;
        }

        public static void SetAudioVolume(float newVolume)
        {
            audioVolume = newVolume;
        }

        public static bool GetAudioEnabled()
        {
            return audioEnabled;
        }

        public static void SetAudioEnabled(bool enable)
        {
            audioEnabled = enable;
        }
    }
}
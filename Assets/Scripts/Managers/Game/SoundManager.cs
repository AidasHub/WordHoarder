using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordHoarder.Resources;
using WordHoarder.Setup;

namespace WordHoarder.Managers.Static.Gameplay
{
    public static class SoundManager
    {
        public enum Sound
        {
            Test,
            Pouf,
            Mirror,
            Fridge,
            Oven,
            Microwave,
            Plant,
            Painting,
            Laptop,
            Cupboard,
            Blender,
            Sink,
            Toaster
        }

        private static AudioSource audioPlayer;

        public static void Init()
        {
            if (audioPlayer == null)
            {
                var audioPlayerGO = new GameObject("AudioPlayer");
                audioPlayer = audioPlayerGO.AddComponent<AudioSource>();
            }
        }

        public static void PlaySound(Sound sound)
        {
            AudioClip audioClip = GameResources.GetAudioClip(sound);
            if (audioClip != null)
            {
                if (audioPlayer == null)
                    Init();
                audioPlayer.PlayOneShot(audioClip);
            }
        }
    }
}
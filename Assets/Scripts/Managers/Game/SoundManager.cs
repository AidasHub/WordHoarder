using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordHoarder.Resources;
using WordHoarder.Setup;
using WordHoarder.Utility;

namespace WordHoarder.Managers.Static.Gameplay
{
    public static class SoundManager
    {
        public enum Sound
        {
            Test,
            Key,
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
            Toaster,
            Window,
            Table,
            Books,
            Magazine,
            Speaker,
            Cactus,
            Guitar,
            TV,
            Fireplace,
            Stool,
            Socket,
            Bathtub,
            Towel,
            ShowerHead,
            Switches,
            Lamp,
            Pillow,
            Purse,
            Trousers,
            Shirt,
            Shelf,
            Shoes
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
            if (SettingsUtility.GetAudioEnabled() == false)
                return;
            AudioClip audioClip = GameResources.GetAudioClip(sound);
            if (audioClip != null)
            {
                if (audioPlayer == null)
                    Init();
                var volume = SettingsUtility.GetAudioVolume();
                audioPlayer.volume = volume;
                audioPlayer.PlayOneShot(audioClip);
            }
        }
    }
}
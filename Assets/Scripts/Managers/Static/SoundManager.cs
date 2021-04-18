using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(audioPlayer == null)
        {
            var audioPlayerGO = new GameObject("AudioPlayer");
            audioPlayer = audioPlayerGO.AddComponent<AudioSource>();
        }
        PlaySound(SoundManager.Sound.Test);
    }

    public static void PlaySound(Sound sound)
    {
        AudioClip audioClip = AssetsManager.GetAudioClip(sound);
        if(audioClip != null)
        {
            audioPlayer.PlayOneShot(audioClip);
        }
    }
}

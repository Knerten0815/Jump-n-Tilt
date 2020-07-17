//Author: Kevin Zielke
using UnityEngine;

namespace AudioControlling
{
    /// <summary>
    /// class for easy usage in the AudioController
    /// </summary>
    [System.Serializable]
    public class Audio
    {
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;

        public Audio(AudioClip audioClip)
        {
            clip = audioClip;
            volume = 1f;
        }

        public Audio(AudioClip audioClip, float vol)
        {
            clip = audioClip;
            volume = vol;
        }
    }
}
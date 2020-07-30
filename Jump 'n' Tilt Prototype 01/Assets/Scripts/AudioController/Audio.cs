//Author: Kevin Zielke
using UnityEngine;

namespace AudioControlling
{
    /// <summary>
    /// If your class needs to play FX Sound, add the namespace AudioControlling and this class as a SerializeField to your variables
    /// and assign it a sound in the inspector. Then play the sound with "AudioController.Instance.playFXSound(Audio sound);"
    /// If you think that the FX Sound is to loud (relative to other FX Sounds) you can turn down the volume in the inspector.
    /// You should not adjust FX volume relative to music volume. That is done in the menu.
    /// See PlayerInput class for example of implementation.
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
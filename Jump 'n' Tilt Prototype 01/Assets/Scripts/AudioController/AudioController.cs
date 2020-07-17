//Author: Kevin Zielke

using GameActions;
using UnityEditor;
using UnityEngine;
using TimeControlls;

namespace AudioControlling
{
    /// <summary>
    /// controls audio in a scene
    /// </summary>
    public class AudioController : MonoBehaviour
    {
        private float musicVol, fxVol;

        [SerializeField] Audio music;
        [SerializeField] Audio attack;
        [SerializeField] Audio earthquake;

        private AudioSource source;

        // initalizing and subscribing to events
        void Awake()
        {
            source = GetComponent<AudioSource>();
            source.clip = music.clip;
            source.volume = music.volume;
            source.Play();
            PlayerInput.onPlayerAttackDown += playAttack;
            PlayerInput.onTiltDown += playEarthquake;
            TimeController.onTimeSpeedChange += slowDownAudio;
            PlayerInput.onSlowMoDown += slowDownAudio;
        }
        private void Update()
        {
            source.volume = music.volume * musicVol;
        }

        // unsubscribing events
        private void OnDisable()
        {
            PlayerInput.onPlayerAttackDown -= playAttack;
            PlayerInput.onTiltDown -= playEarthquake;
            TimeController.onTimeSpeedChange -= slowDownAudio;
            PlayerInput.onSlowMoDown -= slowDownAudio;
        }
        private void playEarthquake(float direction)
        {
            source.PlayOneShot(earthquake.clip, earthquake.volume * fxVol);
        }

        void playAttack()
        {
            source.PlayOneShot(attack.clip, attack.volume * fxVol);
        }

        void slowDownAudio()
        {
            if (source.pitch == 1f)
                source.pitch = 0.8f;
            else
                source.pitch = 1f;
        }

        public void setMusicVolume(float volume)
        {
            musicVol = volume;
        }
        public void setFXVolume(float volume)
        {
            fxVol = volume;
        }
    }
}


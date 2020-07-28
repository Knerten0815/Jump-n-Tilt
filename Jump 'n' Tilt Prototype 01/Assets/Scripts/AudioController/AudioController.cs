//Author: Kevin Zielke

using GameActions;
using UnityEditor;
using UnityEngine;
using TimeControlls;
using UnityEngine.UI;

namespace AudioControlling
{
    /// <summary>
    /// controls audio in a scene
    /// </summary>
    public class AudioController : MonoBehaviour
    {
        private float musicVol, fxVol;
        private AudioSource source;

        [SerializeField] Slider musicSlider;
        [SerializeField] Slider fxSlider;

        [SerializeField] Audio music;
        [SerializeField] Audio attack;
        [SerializeField] Audio earthquake;



        // initalizing and subscribing to events
        void Awake()
        {
            source = GetComponent<AudioSource>();
            source.clip = music.clip;
            source.volume = music.volume;
            source.Play();
            musicVol = 1f;
            fxVol = 1f;

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

        public void setMusicVolume()

        {
            musicVol = musicSlider.value;
        }

  

        public void setFXVolume()

        {
            fxVol = fxSlider.value;
        }
    }

    /*https://www.youtube.com/watch?v=9ROolmPSC70&t=257s

        private static readonly string FirstPlay = "firstPlay";
    private static readonly string BackgroundPref = "BackgroundPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private int firstPlayInt;
    public Slider backgroundSlider, soundEffectsSlider;
    private float backgroundFloat, soundEffectsFloat;

    // Start is called before the first frame update
    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            //Default values for first play
            backgroundFloat = 0.5f;
            soundEffectsFloat = 0.5f;

            //Set the sliders to the default values
            backgroundSlider.value = backgroundFloat;
            soundEffectsSlider.value = soundEffectsFloat;

            //Save the default values for later
            PlayerPrefs.SetFloat(BackgroundPref, backgroundFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);

            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            backgroundFloat = PlayerPrefs.GetFloat(BackgroundPref);
            backgroundSlider.value = backgroundFloat;

            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            soundEffectsSlider.value = soundEffectsFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BackgroundPref, backgroundSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }

    private void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    } */
}


//Author: Kevin Zielke

using GameActions;  //will be deleted, once TimeControls is fixed
using UnityEngine;
using TimeControlls;
using UnityEngine.UI;

namespace AudioControlling
{
    /// <summary>
    /// Singleton class, that plays audio and alters audio playback speed.
    /// If your class needs to play FX Sound, add the namespace AudioControlling and an Audio object as a SerializeField to your variables
    /// and assign it a sound in the inspector. Then play the sound with "AudioController.Instance.playFXSound(Audio sound);"
    /// If you think that the FX Sound is to loud (relative to other FX Sounds) you can turn down the volume in the inspector.
    /// You should not adjust FX volume relative to music volume. That is done in the menu.
    /// See PlayerInput class for example of implementation.
    /// </summary>
    public class AudioController : MonoBehaviour
    {
        private static AudioController _instance;

        public static AudioController Instance { get { return _instance; } }

        private AudioSource source;
        private float musicVol, fxVol;
        private static readonly string firstPlay = "firstPlay";
        private static readonly string musicPref = "musicPref";
        private static readonly string fxPref = "fxPref";
        private int firstPlayInt;

        [SerializeField] Slider musicSlider;
        [SerializeField] Slider fxSlider;

        [SerializeField] Audio music;

        void Awake()
        {

            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            //initalizing variables
            gameObject.AddComponent<AudioSource>();
            source = GetComponent<AudioSource>();
            source.clip = music.clip;
            source.volume = music.volume;
            source.Play();
            firstPlayInt = PlayerPrefs.GetInt(firstPlay);

            //loading audio preferences, checking for first time playing
            if (firstPlayInt == 0)
            {
                //Default values for first play
                musicVol = 0.9f;
                fxVol = 0.7f;

                //Set the sliders to the default values
                musicSlider.value = musicVol;
                fxSlider.value = fxVol;

                //Save the default values for later
                PlayerPrefs.SetFloat(musicPref, musicVol);
                PlayerPrefs.SetFloat(fxPref, fxVol);

                PlayerPrefs.SetInt(firstPlay, -1);
            }
            else
            {
                //Load saved Values from previous games
                musicVol = PlayerPrefs.GetFloat(musicPref);
                musicSlider.value = musicVol;

                fxVol = PlayerPrefs.GetFloat(fxPref);
                fxSlider.value = fxVol;
            }
            source.volume = music.volume * musicVol;

            //subscribing to events
            TimeController.onTimeSpeedChange += slowDownAudio;
            PlayerInput.onSlowMoDown += slowDownAudio;
        }

        public void SaveSoundSettings()
        {
            Debug.Log("saved sound settings");
            PlayerPrefs.SetFloat(musicPref, musicSlider.value);
            PlayerPrefs.SetFloat(fxPref, fxSlider.value);
        }

        private void OnApplicationFocus(bool inFocus)
        {
            if (!inFocus)
            {
                SaveSoundSettings();
            }
        }

        // unsubscribing events
        private void OnDisable()
        {
            TimeController.onTimeSpeedChange -= slowDownAudio;
            PlayerInput.onSlowMoDown -= slowDownAudio;
        }

        //Audio slow down for bullet time
        void slowDownAudio()
        {
            if (source.pitch == 1f)
                source.pitch = 0.8f;
            else
                source.pitch = 1f;
        }

        //set-methods for menu-sliders
        public void updateMusicVolume()
        {
            musicVol = musicSlider.value;
            source.volume = music.volume * musicVol;
        }
        public void setFXVolume()
        {
            fxVol = fxSlider.value;
        }

        //classes can call this method, to play a sound effect
        public void playFXSound(Audio sound)
        {
            source.PlayOneShot(sound.clip, sound.volume * fxVol);
        }

        //classes can call this method, to set the background music
        public void changeMusic(Audio newMusic)
        {
            music = newMusic;
        }
    }
}
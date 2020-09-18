//Author: Kevin Zielke

using UnityEngine;
using TimeControlls;
using UnityEngine.UI;

namespace AudioControlling
{
    /// <summary>
    /// Singleton class, that plays audio, alters audio playback speed and saves audio settings.
    /// If your class needs to play FX Sound, add the namespace AudioControlling and an Audio object as a SerializeField to your variables
    /// and assign it a sound in the inspector. Then play the sound with "AudioController.Instance.playFXSound(Audio sound);"
    /// If you think that the FX Sound is to loud (relative to other FX Sounds) you can turn down the volume in the inspector.
    /// You should not adjust FX volume relative to music volume. That is done in the menu.
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

        [SerializeField] Audio music, timePickUp, heartPickUp, collectibleCardPickUp;
        [SerializeField] AudioClip[] coins;
        [SerializeField] [Range(0f, 1f)] float coinVolume;

        void Awake()
        {
            //Singleton implementation
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            //initalizing variables
            source = gameObject.AddComponent<AudioSource>();
            source.clip = music.clip;
            source.volume = music.volume;
            source.loop = true;
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
        }

        /// <summary>
        /// Saves the sound settings in the save file.
        /// </summary>
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
            //PlayerInput.onSlowMoDown -= slowDownAudio;
        }

        /// <summary>
        /// Audio slow down effect for bullet time.
        /// </summary>
        void slowDownAudio()
        {
            if (source.pitch == 1f)
                source.pitch = 0.8f;
            else
                source.pitch = 1f;
        }

        /// <summary>
        /// The music slider in the sound menu subscribes to this class, so that the music volume is updated.
        /// </summary>
        public void updateMusicVolume()
        {
            musicVol = musicSlider.value;
            source.volume = music.volume * musicVol;
        }
        /// <summary>
        /// The effects slider in the sound menu subscribes to this class, so that the effect volume is updated.
        /// </summary>
        public void setFXVolume()
        {
            fxVol = fxSlider.value;
        }
        /// <summary>
        /// This method can be called to play an effect sound.
        /// </summary>
        /// <param name="sound">This sound will be played</param>
        public void playFXSound(Audio sound)
        {
            source.PlayOneShot(sound.clip, sound.volume * fxVol);
        }

        //Collectibles don't call playFXSound(Audio sound). Instead they call the corresponding method in the AudioController.
        //This way we can avoid having lots of links between the many collectibles in the levels and audio files and save performance

        public void playTimePickUpFX()
        {
            source.PlayOneShot(timePickUp.clip, timePickUp.volume * fxVol);
        }

        public void playHeartPickUpFX()
        {
            source.PlayOneShot(heartPickUp.clip, heartPickUp.volume * fxVol);
        }

        public void playCollectibleCardPickUpFX()
        {
            source.PlayOneShot(collectibleCardPickUp.clip, collectibleCardPickUp.volume * fxVol);
        }
        /// <summary>
        /// Plays a random coin sound out of 8 possible sounds.
        /// </summary>
        public void playCoinPickUpFX()
        {
            source.PlayOneShot(coins[Random.Range(0, 8)], coinVolume * fxVol);
        }
    }
}
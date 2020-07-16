//Author: Kevin Zielke

using GameActions;
using UnityEditor;
using UnityEngine;
using TimeControlls;

/// <summary>
/// controls audio in a scene
/// </summary>
public class AudioController : MonoBehaviour
{
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
        source.volume = music.volume;        
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
        source.PlayOneShot(earthquake.clip, earthquake.volume);
    }

    void playAttack()
    {
        source.PlayOneShot(attack.clip, attack.volume);
    }

    void slowDownAudio()
    {
        if (source.pitch == 1f)
            source.pitch = 0.8f;
        else
            source.pitch = 1f;
    }
}

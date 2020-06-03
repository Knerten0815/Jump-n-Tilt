using GameActions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioController : MonoBehaviour
{
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip earthquake;
    [SerializeField] AudioClip music;

    private AudioSource source;

    // Start is called before the first frame update
    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = music;
        source.Play();
        PlayerInput.onPlayerAttackDown += playAttack;
        PlayerInput.onTiltDown += playEarthquake;
    }

    private void playEarthquake(float direction)
    {
        source.PlayOneShot(earthquake);
    }

    private void OnDisable()
    {
        PlayerInput.onPlayerAttackDown -= playAttack;
    }

    void playAttack()
    {
        source.PlayOneShot(attack);
    }
}

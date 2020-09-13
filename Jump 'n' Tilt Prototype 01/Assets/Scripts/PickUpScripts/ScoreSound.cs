using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoreSound : MonoBehaviour, PickUpDescriptor
{

    public void hit() 
    {
        AudioController.Instance.playCoinPickUpFX();
    }

}

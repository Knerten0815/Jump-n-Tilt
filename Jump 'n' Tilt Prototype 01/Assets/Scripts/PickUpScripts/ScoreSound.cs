using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for component to play the coin pickup fx
//author: Katja Tuemmers
public class ScoreSound : MonoBehaviour, PickUpDescriptor
{

    public void hit() 
    {
        AudioController.Instance.playCoinPickUpFX();
    }

}

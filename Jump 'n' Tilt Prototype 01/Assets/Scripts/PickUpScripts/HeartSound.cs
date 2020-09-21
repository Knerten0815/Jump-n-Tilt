using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//class for component to play the heart pickup fx
//author: Katja Tuemmers
public class HeartSound: MonoBehaviour, PickUpDescriptor
{

    public void hit()
    {
        AudioController.Instance.playHeartPickUpFX();
    }

}


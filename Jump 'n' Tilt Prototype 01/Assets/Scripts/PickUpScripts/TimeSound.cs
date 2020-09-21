using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//class for component to play the time pickup fx
//author: Katja Tuemmers
public class TimeSound: MonoBehaviour, PickUpDescriptor
{

    public void hit()
    {
        AudioController.Instance.playTimePickUpFX();
    }

}


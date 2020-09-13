using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TimeSound: MonoBehaviour, PickUpDescriptor
{

    public void hit()
    {
        AudioController.Instance.playTimePickUpFX();
    }

}


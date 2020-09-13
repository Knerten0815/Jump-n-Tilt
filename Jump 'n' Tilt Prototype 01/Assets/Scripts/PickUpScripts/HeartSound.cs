using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartSound: MonoBehaviour, PickUpDescriptor
{

    public void hit()
    {
        AudioController.Instance.playHeartPickUpFX();
    }

}


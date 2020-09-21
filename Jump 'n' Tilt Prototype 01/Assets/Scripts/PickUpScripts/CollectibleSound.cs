using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//class for component to play the collectible fx
//author: Katja Tuemmers
public class CollectibleSound : MonoBehaviour, PickUpDescriptor
{

    public void hit()
    {
        AudioController.Instance.playCollectibleCardPickUpFX();
    }

}


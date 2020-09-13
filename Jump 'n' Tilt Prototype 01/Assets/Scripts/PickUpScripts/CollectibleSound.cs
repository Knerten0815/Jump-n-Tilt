using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSound : MonoBehaviour, PickUpDescriptor
{

    public void hit()
    {
        AudioController.Instance.playCollectibleCardPickUpFX();
    }

}


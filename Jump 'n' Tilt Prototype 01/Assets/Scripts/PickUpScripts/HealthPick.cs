using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//class for component to trigger the event in ManagementSystem to notify objects a heart pickup has been picked up 
//Author:Katja Tuemmers
public class HealthPick : MonoBehaviour, PickUpDescriptor
{

    public void hit()
    {
        ManagementSystem.Instance.hUp();
    }

 
}

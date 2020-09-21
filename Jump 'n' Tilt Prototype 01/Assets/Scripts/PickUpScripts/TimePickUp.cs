using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//class for component to trigger the event in ManagementSystem to notify objects a time pickup has been picked up 
//author: Katja Tuemmers
public class TimePickUp : MonoBehaviour, PickUpDescriptor
{

    public void hit()
    {
      ManagementSystem.Instance.tUp();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickUp : MonoBehaviour, PickUpDescriptor
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void hit()
    {
      ManagementSystem.tUp();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*
* Component class for PowerUps.
*
* @Katja
*/
public class PowerUp : MonoBehaviour, PickUpDescriptor
{
    public void hit()
    {
        Debug.Log("B");
    }

}

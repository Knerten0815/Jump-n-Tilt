using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*
* Component class for common PickUps, like Points/Coins etc.
*
* @Katja
*/
public class Common : MonoBehaviour, PickUpDescriptor
{
    /*
    *
    * triggers Event in ManagementSystem
    *
    * @Katja
    */
    public void hit()
    {
        ManagementSystem.pickUp();
    }

}

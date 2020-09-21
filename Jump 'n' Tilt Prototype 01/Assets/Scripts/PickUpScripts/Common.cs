using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*
* Component class for coin PickUps 
*
* Author:Katja Tuemmers
*/
public class Common : MonoBehaviour, PickUpDescriptor
{
  
    public int Value; //Point value of the pick up
  /*
  *
  * triggers Event in ManagementSystem
  *
  * @Katja
  */

    public void hit()
    {
        ManagementSystem.Instance.pickUp(Value);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Basic Class for all PickUp Objects. To define what kind of PickUp it is additional Componentents like Rare, Common or PowerUps are added 
* to the game Object
*
*/
public class PickUp : MonoBehaviour
{

    /*
     * When a PickUp was hit by the Player character the PickUp Object loops through all its possible Descriptors and calls their individual
     * hit functions. E.g a common descriptor then does different thing than the collectible one which also saves the game etc. It is possible
     * to combine these. Say Common and Collectible which right now would mean you also get points for gathering the collectible
     *
     * 
     * @Katja
     */
    public void hit()
    {
        PickUpDescriptor[] descriptors = GetComponents<PickUpDescriptor>();
        foreach (PickUpDescriptor d in descriptors)
        {
            d.hit();
        }
            
    }

  
}

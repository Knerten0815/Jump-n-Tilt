using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*
* Component class for rare PickUps like collectibles
*
* @Katja
*/
public class Rare : MonoBehaviour, PickUpDescriptor
{
    /*
     * Unique ID for a Collectibel to identify it for save and loading purposes
     *
     * @Katja
     */

    [SerializeField]
    private int numberID;

    /*
    * Calls for the Collectible ID to be added to the current List of Collectibles and also to save that current List by calling
    * SaveGame()
    *
    *@Katja
    */
    public void hit()
    {
        ManagementSystem.AddCollectible(numberID);
        ManagementSystem.SaveGame();
    }

    /*
    * Subscribed to the checkForCollected Event called during loading of a save File. It sets all Collectibles whose ID has already been
    * saved to inactive
    *
    *@Katja
    */
    public void checkForCollected(int collectibleID)
    {
        
            if (collectibleID == numberID)
            {
                gameObject.SetActive(false);
            }
        
    }

    /*
    * Subcribes to checkForCollected upon waking
    * 
    * @Katja
    */
    void Awake()
    {
        ManagementSystem.collectibleOnLoad += checkForCollected;
    }



}

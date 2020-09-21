using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
* Author: Katja Tuemmers
* Component class for collectible PickUps
* 
*/
public class Rare: MonoBehaviour, PickUpDescriptor
{
    /*
     * Unique ID for a Collectibel to identify it for save and loading purposes
     *
     * 
     */

    [SerializeField]
    private int numberID;



    /*
    * Calls for the Collectible ID to be added to the current List of Collectibles, to update the scroll Display in the GUI and also to save that current List by calling
    * SaveGame()
    *
    *
    */
    public void hit()
    {
        ManagementSystem.Instance.addCollectible(numberID);
        ManagementSystem.Instance.collectedUpdate();
        ManagementSystem.Instance.SaveGame();
    }

    /*
    * Subscribed to the checkForCollected Event called during loading of a save File. All collectibles listen and if their ID is passed on 
    * in the event they start modifying themselves by adding the ScoreSound component changing their spire and then deleting their 
    * collectible exclusive components
    *
    *
    */
    public void checkForCollected(int collectibleID)
    {
        
            if (collectibleID == numberID && this != null)
                {
                ScoreSound scoreSound = gameObject.AddComponent(typeof(ScoreSound)) as ScoreSound;
                SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
                ManagementSystem.Instance.collectedUpdate();

                sp.sprite = Resources.Load<Sprite>("PickUp Sprites/GoldCoinWithRuby_64x64");
                Rare rare = gameObject.GetComponent<Rare>();
                CollectibleSound collectibelSound= gameObject.GetComponent<CollectibleSound>();
                Object.Destroy(collectibelSound);
                Object.Destroy(rare);
                }
        
    }

    /*
    * Subcribes to checkForCollected upon OnEnabling
    * 
    *
    */
    void OnEnable()
    {
        ManagementSystem.Instance.collectibleOnLoad += checkForCollected;
    }

    private void OnDisable()
    {
        ManagementSystem.Instance.collectibleOnLoad -= checkForCollected;

    }

}





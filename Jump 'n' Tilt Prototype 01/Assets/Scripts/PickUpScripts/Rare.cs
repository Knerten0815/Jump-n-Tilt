using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*
* Component class for rare PickUps like collectibles
*
* @Katja
*/
public class Rare: MonoBehaviour, PickUpDescriptor
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
        ManagementSystem.Instance.AddCollectible(numberID);
        ManagementSystem.Instance.collectedUpdate();
        ManagementSystem.Instance.SaveGame();
    }

    /*
    * Subscribed to the checkForCollected Event called during loading of a save File. It sets all Collectibles whose ID has already been
    * saved to inactive
    *
    *@Katja
    */
    public void checkForCollected(int collectibleID)
    {
        
            if (collectibleID == numberID && this != null)
                {
                //gameObject.SetActive(false);
                Common common = gameObject.AddComponent(typeof(Common)) as Common;
                common.Value=40;
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
    * Subcribes to checkForCollected upon waking
    * 
    * @Katja
    */
    void Awake()
    {
        ManagementSystem.Instance.collectibleOnLoad += checkForCollected;
    }

    private void OnDisable()
    {
        ManagementSystem.Instance.collectibleOnLoad -= checkForCollected;

    }

}





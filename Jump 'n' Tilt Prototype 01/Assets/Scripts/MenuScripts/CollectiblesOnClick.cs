using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Author: Katja Tuemmers
//Component added to buttons in the Collectibles Menu, loads the CollectibleCard and calls for the DisplayScreen to be updated
//Closes the Collectibles Menu and opens the DisplyaScreen
public class CollectiblesOnClick : MonoBehaviour
{
    public DisplayCollectible display;
    /*
    * Switches from Collectibles Menu to DisplayScreen 
    * Can only happen if the button contains a proper name
    * Is called onClick and set in Inspector
    */
    public void GetCollectibles(GameObject collected)
    {
        if(this.GetComponentInChildren<TextMeshProUGUI>().text != "???")
        {
            this.transform.parent.gameObject.SetActive(false);
            collected.SetActive(true);
        }
    }
    /*
    *
    *
    * Loads the desired collectible card and unloads the old one
    * CollectibleCard and Button need to having matching names.
    * 
    */
    public void SetCard()
    {

        //unloads collectible card before a new one is loaded
        if (display.collectible != null)
        {
            Resources.UnloadAsset(display.collectible as Object);
            display.collectible = null;
        }

        //Loads collectible card from resources
        CollectibleCard collectable = Resources.Load("Collectible Cards/" + this.GetComponentInChildren<TextMeshProUGUI>().text) as CollectibleCard;
        display.collectible = collectable; //Set newly loaded collectible card
        display.update();//The text and image in Display Screen is updated to use the new collectible card

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectiblesOnClick : MonoBehaviour
{
    public DisplayCollectible display;
    public CollectibleCard collectible;
    public void GetCollectibles(GameObject collected)
    {
        if(this.GetComponentInChildren<TextMeshProUGUI>().text != "???")
        {
            this.transform.parent.gameObject.SetActive(false);
            collected.SetActive(true);
        }
    }
    public void SetCard()
    {
        if (display.collectible != null)
        {
            Resources.UnloadAsset(display.collectible as Object);
            display.collectible = null;
            Debug.Log("Unloaded");
        }
           CollectibleCard collectable = Resources.Load("Collectible Cards/" + this.GetComponentInChildren<TextMeshProUGUI>().text) as CollectibleCard;
            //Debug.Log(collectable.title);
            display.collectible = collectable;
            display.update();
        
    }
}

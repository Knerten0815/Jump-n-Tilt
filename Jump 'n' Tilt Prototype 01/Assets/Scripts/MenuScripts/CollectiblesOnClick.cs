using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectiblesOnClick : MonoBehaviour
{
    public DisplayCollectible display;

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
        CollectibleCard collectable = Resources.Load("Collectible Cards/Kashima") as CollectibleCard;
        Debug.Log(collectable.title);
        display.collectible = collectable;
        display.update();
    }
}

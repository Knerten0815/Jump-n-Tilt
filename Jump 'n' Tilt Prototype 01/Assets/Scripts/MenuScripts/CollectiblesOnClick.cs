using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectiblesOnClick : MonoBehaviour
{
    public void GetCollectibles(GameObject collected)
    {
        if(this.GetComponentInChildren<TextMeshProUGUI>().text != "???")
        {
            this.transform.parent.gameObject.SetActive(false);
            collected.SetActive(true);
        }
    }
}

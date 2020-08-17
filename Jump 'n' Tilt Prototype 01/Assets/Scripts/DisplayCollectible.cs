using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using TMPro;

public class DisplayCollectible : MonoBehaviour
{

    public CollectibleCard collectible;
    
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Hashtable buttons = new Hashtable();



    public RawImage image;


    // Start is called before the first frame update
    void Start()
    {
       // ManagementSystem.collectibleOnLoad += overwrite;
    }

    public void overwrite(int ID)
    {
        /*
        title.text = collectible.title;
        description.text = collectible.description;
        image.sprite = collectible.artwork;
        */
    }
    public void update()
    {
        title.text = collectible.title;
        description.text = collectible.description;
        image.texture = collectible.artwork.texture;
    }
}

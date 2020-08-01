using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using TMPro;

public class DisplayCollectible : MonoBehaviour
{
    [SerializeField]
    public CollectibleCard collectible;
    public int ID;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    
  


    public Image image;


    // Start is called before the first frame update
    void Start()
    {
        title.text = "????";
        description.text = "????";
        image.sprite = collectible.artwork;
        ManagementSystem.collectibleOnLoad += overwrite;


    }

    public void overwrite(int ID)
    {
        title.text = collectible.title;
        description.text = collectible.description;
        image.sprite = collectible.artwork;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[CreateAssetMenu]
public class CollectibleCard : ScriptableObject
{
    // Start is called before the first frame update
    public new string name;
    public string title;
    public string description;

    public Sprite artwork;

}

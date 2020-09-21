using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Katja Tuemmers
//Collectible Cards can be easily created by right clicking under the create menu
[CreateAssetMenu]
public class CollectibleCard : ScriptableObject
{
    public new string name;
    public string title;
    public string description;
    public Sprite artwork;
}

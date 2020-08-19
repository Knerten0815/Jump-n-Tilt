using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Michelle Limbach
[System.Serializable]
public class Dialogue
{
    [TextArea(3, 20)]
    public string[] name;
    [TextArea(3, 20)]
    public string[] sentences;
    [Tooltip("Max Array length 4!")]
    public GameObject[] images = new GameObject[3];
}

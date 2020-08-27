using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Michelle Limbach
[System.Serializable]
public class Dialogue
{
    [TextArea(3, 30)]
    public string[] name;
    [TextArea(3, 30)]
    public string[] sentences;
    [Tooltip("Max Array length 5!")]
    public GameObject[] images = new GameObject[5];
}

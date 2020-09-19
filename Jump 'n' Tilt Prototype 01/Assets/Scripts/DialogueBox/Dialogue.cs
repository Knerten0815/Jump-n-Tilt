using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Michelle Limbach
//Dialogue object which can be edited in the inspector.
//Contains all names and sentences that should be displayed in the dialogue boxes via the DialogueManager
[System.Serializable]
public class Dialogue
{
    [TextArea(3, 30)]
    public string[] name;
    [TextArea(3, 30)]
    public string[] sentences;
}

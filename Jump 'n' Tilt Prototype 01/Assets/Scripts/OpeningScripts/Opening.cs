using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Michelle Limbach
//Opening object which can be edited in the inspector.
//Contains all parapraphs that should be displayed in the Opening scene via the OpeningManager
[System.Serializable]
public class Opening
{
    [TextArea(3, 20)]
    public string[] paragraphs;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : Hazards 
{ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("hit");
        Attack();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Melanie Jäger
//Class for spike enemy
public class Spikes : Hazards 
{ 
    //A Collison causes the spike to attack
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Attack();
    }
}

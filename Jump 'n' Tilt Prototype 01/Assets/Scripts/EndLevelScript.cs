using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Katja Tuemmers
//Triggers the end of the level when player enters the BoxCollider, ManagementSystem then continues to open the HighScore scene
public class EndLevelScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ManagementSystem.Instance.endLevel();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Katja Tuemmers
/*
* ObjectSpawning activates and deactivates objects depending on the position of the player to help with performance issues in 
* Level 3. The ObjectSpawning needs to different Arrays of GameObjects and a BoxCollider with a trigger to work.
* 
* It saves the playerPosition when the player enters the BoxCollider. Then when the player leaves the collider it compares the current
* position with the saved one to decide into which direction the player is leaving the Box, towards the latter half of the game or back to
* the beginning. If the player moves into the negative direction of the x-axis the elements of the myObjectsPast array are activated and 
* myObjectsFuture deactivated.
* If the player leaves towards the positive direction of the x-axis the objects in myObjectsFuture are activated. 
*/
public class ObjectSpawning : MonoBehaviour
{
    public GameObject[] myObjectsFuture; //Objects in the latter half of the game
    public GameObject[] myObjectsPast; //Objects in the first half of the game
    private Vector3 playerPositionPrev; //Saves player position on entering the trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            playerPositionPrev = player.GetComponent<Rigidbody2D>().transform.position; //Player Position is saved
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            Vector3 playerPositionCurrently = player.GetComponent<Rigidbody2D>().transform.position;
            if (playerPositionPrev.x < playerPositionCurrently.x) //Player moves in the positive direction
            {
                playerPositionPrev = player.GetComponent<Rigidbody2D>().transform.position;
                foreach (GameObject objects in myObjectsFuture)
                {
                    objects.SetActive(true);
                }
                foreach (GameObject objects in myObjectsPast)
                {
                    objects.SetActive(false);
                }

            }
            else //player moves in the negative direction or leaves the box in the position he entered
            {
                foreach (GameObject objects in myObjectsFuture)
                {
                    objects.SetActive(false);
                }
                foreach (GameObject objects in myObjectsPast)
                {
                    objects.SetActive(true);
                }


            }
        }
    }

}

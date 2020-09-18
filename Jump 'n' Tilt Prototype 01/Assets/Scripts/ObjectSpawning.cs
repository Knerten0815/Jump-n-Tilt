using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] myObjectsFuture;
    public GameObject[] myObjectsPast;
    private Vector3 playerPositionPrev;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            playerPositionPrev = player.GetComponent<Rigidbody2D>().transform.position;
            /*foreach (GameObject spike in myObjectsFuture)
            {
                spike.SetActive(!spike.activeSelf);
            }
            foreach (GameObject spike in myObjectsPast)
            {
                spike.SetActive(!spike.activeSelf);
            }*/
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerCharacter player = collision.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            Vector3 playerPositionCurrently = player.GetComponent<Rigidbody2D>().transform.position;
            if (playerPositionPrev.x < playerPositionCurrently.x)
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
            else
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

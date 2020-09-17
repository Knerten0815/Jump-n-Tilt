using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawning : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] myObjectsFuture;
    public GameObject[] myObjectsPast;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerCharacter>() != null)
        {
            foreach (GameObject spike in myObjectsFuture)
            {
                spike.SetActive(!spike.activeSelf);
            }
            foreach (GameObject spike in myObjectsPast)
            {
                spike.SetActive(!spike.activeSelf);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;

//Author: Melanie Jäger
//Class for shooting arrows at regular intervals
public class SpawnArrows : MonoBehaviour
{
    public GameObject arrowPrefab;          //arrowprefab needs to be assigned in the inspector
    public float spawnTime = 7.0f;          //amount of time that passes until a new arrow is spawned (in seconds)
    public float deltaTimeAdjust = 200.0f;  //varialbe, that calculates a rational result for the time between the spawns, with and without the TimeController activated

    private TimeController timeController;
    public GameObject arrow;
    public bool shoot = true;

    //Author: Melanie Jäger
    private void OnEnable()
    {
        shoot = true;
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    //Author: Melanie Jäger
    //starts the couroutine
    void Start()
    {
        StartCoroutine(ShootingArrows());
    }

    //Author: Melanie Jäger
    //Spawns the arrow prefab at the position of this gameobject, normally it would be the device that shoots the arrows
    private void SpawnArrow()
    {
        if (shoot)      //Devices at boss fight are supposed to shoot their arrows after the kitsune is hit several times
        {
            arrow = Instantiate(arrowPrefab) as GameObject;
            arrow.transform.position = transform.position;
        }
    }

    //Author: Melanie Jäger
    IEnumerator ShootingArrows()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime - (timeController.getSpeedAdjustedDeltaTime() * deltaTimeAdjust));
            SpawnArrow();
        }
    }
}

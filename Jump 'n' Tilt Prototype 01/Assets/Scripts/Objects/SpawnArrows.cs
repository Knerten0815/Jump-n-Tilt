using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;
using AudioControlling;


//Author: Melanie Jäger
//Class for shooting arrows at regular intervals
public class SpawnArrows : MonoBehaviour
{
    public GameObject arrowPrefab;          //arrowprefab needs to be assigned in the inspector
    public float spawnTime = 7.0f;          //amount of time that passes until a new arrow is spawned (in seconds)
    public float deltaTimeAdjust = 200.0f;  //varialbe, that calculates a rational result for the time between the spawns, with and without the TimeController activated

    public TimeController timeController;
    public GameObject arrow;
    public bool shoot = true;  //added by Katja Tuemmers to prevent constant spawning of arrows when the player is far away
    public Audio arrowSound;
    public Rigidbody2D player;
    public Vector3 ownPosition;

    //Author: Melanie Jäger
    public virtual void OnEnable()
    {
        shoot = true;
        ownPosition = this.GetComponent<BoxCollider2D>().transform.position;
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
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
        float distance = Vector2.Distance(ownPosition, player.position);
        if (shoot && distance < 50) //if statement added by Katja Tuemmers to check whether the player is near enough before spawning arrow and playing sound
        {
            AudioController.Instance.playFXSound(arrowSound);
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

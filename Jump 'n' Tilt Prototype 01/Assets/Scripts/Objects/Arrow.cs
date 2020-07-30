using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;
using LevelControlls;

public class Arrow : PhysicsObject
{
    private TimeController timecontroller;
    private LevelControllerNew levelController;
    private Transform level;
    private float moveDirection;
    private float moveAngle;
    private Quaternion startAngle;

    private void OnEnable()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        levelController = GameObject.Find("LevelController").GetComponent<LevelControllerNew>();
        level = GameObject.Find("Level").transform;
    }

    private void Start()
    {
        startAngle = level.rotation;

        transform.rotation = startAngle;

        moveAngle = levelController.tiltAngle / 45f;
        moveDirection = levelController.getStep() * moveAngle;
    }

    void Update()
    {
        Movement();
        Debug.Log(level.rotation.z);
    }

    //Object is moving non stop
    private void Movement()
    {
        transform.position = transform.position + new Vector3(maxSpeed, -maxSpeed * moveDirection, 0) * timeController.getSpeedAdjustedDeltaTime();
    }

    private void Rotation()
    {
        if(level.rotation != startAngle)
        {
            if(level.rotation.z < 0) 
            {

            }
            else if(level.rotation.z < 0)
            {

            }
            startAngle = level.rotation;
        }
    }

        

    //Collision with player destroys the player
    //Collision with any other object destroys this object
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            Destroy(collision.gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

 
}

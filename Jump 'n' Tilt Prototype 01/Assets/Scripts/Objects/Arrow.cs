using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;
using LevelControlls;

//Author: Melanie Jäger
//Class for arrow enemy
public class Arrow : Hazards
{
    private Transform level;
    private float moveDirection;                //Variable for correct moving direction; positive is up and negative is down
    private float moveAngle;                    //Variable for the correct moving angle according to the y-axis

    private float cameraPos;                    //Position of the camera

    public float destroyDistance = 10.0f;       //distance from the camera position until the arrow is destroyed
    public int spawnDistance = 12;              //variable to multiply the distance from the camera position until the arrow is spawned

    //sets the basic values for the arrow movement
    private new void Start()
    {
        base.Start();
        level = GameObject.Find("Level").transform;
 
        transform.rotation = level.rotation;        //sets the correct rotation for the arrow  

        moveAngle = levelController.tiltAngle / 45f;                //calculates the angle, the arrows needs to go
        moveDirection = levelController.getTiltStep() * moveAngle;  //calculates if the arrow is going up or down
    }

    //updates the cameraposition and calls the functions
    void FixedUpdate()
    {
        cameraPos = Camera.main.transform.position.x;

        Movement();
        DestoryObject();
    }

    //movement for the arrow
    private void Movement()
    {
        transform.position = transform.position + new Vector3(maxSpeed, -maxSpeed * moveDirection, 0) * timeController.getSpeedAdjustedDeltaTime();
    }

    //destroys the arrow object when a certain distance is between it and the cameraposition --> the object gets destroyed when it is out of the visible part of the level
    //distance can be changed in the inspector with the variables destroyDistance and spawnDistance
    private void DestoryObject()
    {
        if(transform.position.x < cameraPos - destroyDistance || transform.position.x > cameraPos + destroyDistance * spawnDistance)
        {
            Destroy(gameObject);
        }
    }

    //Collision with player takes one health point from the player
    //Collision with any other object destroys this object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Attack();
        }
        else
        {
            Destroy(gameObject);
        }        
    }
}

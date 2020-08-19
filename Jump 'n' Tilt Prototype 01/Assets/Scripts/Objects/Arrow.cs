using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;
using LevelControlls;

//Author: Melanie Jäger
public class Arrow : Character
{
    private Transform level;
    private new float moveDirection;
    private float moveAngle;

    private float cameraPos;

    public float destroyDistance = 10.0f;   //distance from the camera position until the arrow is destroyed
    public int spawnDistance = 3;           //variable to multiply the distance from the camera position until the arrow is spawned

    //Author: Melanie Jäger
    //sets the basic values for the arrow movement
    private new void Start()
    {
        base.Start();   //calls the start function of the parent class first

        level = GameObject.Find("Level").transform;

        //sets the direction for the arrow movement
        transform.rotation = level.rotation;    


        moveAngle = levelController.tiltAngle / 45f;       //calculates the angle, the arrows needs to go
        moveDirection = levelController.getTiltStep() * moveAngle;  //calculates if the arrow is going up or down

    }

    //Author: Melanie Jäger
    //updates the cameraposition and calls the functions
    void Update()
    {
        cameraPos = Camera.main.transform.position.x;

        Movement();
        DestoryObject();
    }

    //Author: Melanie Jäger
    //movement for the arrow
    private void Movement()
    {
        transform.position = transform.position + new Vector3(maxSpeed, -maxSpeed * moveDirection, 0) * timeController.getSpeedAdjustedDeltaTime();
    }

    //Author: Melanie Jäger
    //destroys the arrow object when a certain distance is between it and the cameraposition --> so there a not a million arrows in the scene
    //distance can be changed in the inspector with the variables destroyDistance and spawnDistance
    private void DestoryObject()
    {
        if(transform.position.x < cameraPos - destroyDistance || transform.position.x > cameraPos + destroyDistance * spawnDistance)
        {
            Destroy(gameObject);
        }
    }


    //Author: Melanie Jäger
    //Collision with player takes one health point from the player
    //Collision with any other object destroys this object
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            //Debug.Log("hit");
            Attack();
        }

        else
        {
            Destroy(gameObject);
        }        
    }
}

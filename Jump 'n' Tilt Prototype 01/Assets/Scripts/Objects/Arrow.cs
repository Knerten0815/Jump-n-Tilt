using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;
using LevelControlls;

//Author: Melanie Jäger
public class Arrow : PhysicsObject
{
    private Transform level;
    private float moveDirection;
    private float moveAngle;

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
    void Update()
    {
        Movement();
    }

    //Author: Melanie Jäger
    //movement for the arrow
    private void Movement()
    {
        transform.position = transform.position + new Vector3(maxSpeed, -maxSpeed * moveDirection, 0) * timeController.getSpeedAdjustedDeltaTime();
    }

    //Author: Melanie Jäger
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

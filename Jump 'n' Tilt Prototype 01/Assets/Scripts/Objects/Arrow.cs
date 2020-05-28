using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;

public class Arrow : PhysicsObject
{
    private TimeController timecontroller;
    // Start is called before the first frame update
    private void OnEnable()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    //Object is moving non stop
    private void Movement()
    {
        transform.position = transform.position + new Vector3(maxSpeed * timeController.getSpeedAdjustedDeltaTime(), 0, 0);
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

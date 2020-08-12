using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelControlls;
using TimeControlls;

//Author: Melanie Jäger
public class Barrel : PhysicsObject
{
    public float gravity = 50.0f;   //variable that is multiplied with the adjusted deltaTime, so that the object slows down when TimeController is activated

    //Author: Melanie Jäger
    //objects gets adjusted gravity, that regulates the speed
    private void AdjustGravity()
    {
        rb2d.gravityScale = gravity * timeController.getSpeedAdjustedDeltaTime(); 
    }

    //Author: Melanie Jäger
    void Update()
    {
        AdjustGravity();
    }
}

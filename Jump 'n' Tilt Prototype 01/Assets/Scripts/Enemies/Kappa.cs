using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Nicole Mynarek
public class Kappa : GroundEnemy
{

    //private float speed; 

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //speed = moveSpeed;
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        //patrol
        //moveSpeed = speed;
        if (IsWallAhead(false) == true || isGroundAhead(true) == false)
        {

            if (isFacingRight == true)
            {
                direction = -1;
                isFacingRight = false;
            }
            else
            {
                direction = 1;
                isFacingRight = true;
            }
        }

        Movement(direction);
    }
}

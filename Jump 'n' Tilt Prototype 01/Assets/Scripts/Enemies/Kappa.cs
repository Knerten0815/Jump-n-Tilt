using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Nicole Mynarek
public class Kappa : GroundEnemy
{

    //private float speed; 
    public float lastYPos = 0;
    public float idleTime = 0.2f;
    public float currentIdleTime = 0;
    public bool isIdle = true;
    public bool isJumping = false;
    public bool isFalling = false;
    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lastYPos = transform.position.y;
        attackRadius = 1.3f;
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        //Debug.Log("isSliding? " + isSliding);

        //patrol
        if (isSliding)
        {
            isIdle = false;
            isJumping = false;
            isFalling = false;
            Slide();
            //Debug.Log("Kappa slided");
        }
        //else if (grounded)
        //{
            else if (IsWallAhead() == true || isGroundAhead() == false)
            {
                hasAttacked = false;
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
        //}

        if (isIdle)
        {
            currentIdleTime += Time.deltaTime;

            if(currentIdleTime >= idleTime)
            {
                currentIdleTime = 0;
                //isFacingRight = !isFacingRight;
                Jump();
            }
        }

        if(grounded == true && isJumping == false)
        {
            //Debug.Log("grounded? " + grounded);

            isIdle = true;
            isJumping = false;
            isFalling = false;
        }
        else if(transform.position.y > lastYPos && grounded == false && isIdle == false)
        {
            isJumping = true;
            isFalling = false;
        }
        else if(transform.position.y < lastYPos && grounded == false && isIdle == false)
        {
            isJumping = false;
            isFalling = true;
        }

        lastYPos = transform.position.y;
    }

    protected override void Jump()
    {
        
            isSliding = false;
            isIdle = false;
            isJumping = true;

            if (isFacingRight == true)
            {
                direction = 1f;
                CharacterFacingDirection(direction);
            }
            else
            {
                direction = -1f;
                CharacterFacingDirection(direction);
            }

            velocity = new Vector2(6 * direction, jumpHeight);

            //Debug.Log("Kappa Jump");
        
    }
    
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackPos.position, attackRadius);
    }
}

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

    //public LayerMask whatIsPlatform = LayerMask.GetMask("Platform");


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

        

        if (isIdle && playerDirection().x >= 20f)
        {
            currentIdleTime += Time.deltaTime;

            if(currentIdleTime >= idleTime)
            {
                currentIdleTime = 0;
                isFacingRight = !isFacingRight;
                Jump();
            }
        }
        else if (isIdle && playerDirection().x < 20f)
        {   
            if(platformAhead() != Vector2.zero)
            {
                Debug.Log(platformAhead());
                Debug.Log("Da ist eine Plattform!!!");

                Vector2 platformDirection = platformAhead();
                jumpHeight += Mathf.Abs(platformDirection.y);
            }

            currentIdleTime += Time.deltaTime;

            if(currentIdleTime >= idleTime)
            {
                currentIdleTime = 0;
                Jump();
            }
        }

        if(grounded == true && isJumping == false)
        {
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

        if (playerDirection().x >= 20f)
        {
            Debug.Log("Player wird NICHT verfolgt!!!!");
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
        }

        velocity = new Vector2(6 * direction, jumpHeight);

        if(playerDirection().x < 20f)
        {
            Debug.Log("player wird verfolgt");
            Debug.Log("Playerdirection: " + playerDirection().x);

            if(playerDirection().x < 0f)
            {
                direction = -1f;
                //isFacingRight = false;
                CharacterFacingDirection(direction);
            }
            else
            {
                direction = 1f;
                //isFacingRight = true;
                CharacterFacingDirection(direction);
            }
            velocity = new Vector2(6 * playerDirection().x, jumpHeight);
        }
    }

    /*bool platformAhead()
    {
        RaycastHit2D platformAhead;
        Debug.DrawRay(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.up * 10f);
        platformAhead = Physics2D.Raycast(new Vector2(transform.position.x + 0.5f, transform.position.y), Vector2.up, 10f, whatIsPlatform);

        if (platformAhead.collider)
        {
            return true;
        }

        return false;
    }*/

    /*void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackPos.position, attackRadius);
    }*/
}

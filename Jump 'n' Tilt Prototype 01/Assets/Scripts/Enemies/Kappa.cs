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
        //speed = moveSpeed;
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        //Debug.Log("isSliding? " + isSliding);

        //patrol
        //moveSpeed = speed;
        //if (grounded)
        //{
            if (IsWallAhead(false) == true || isGroundAhead(true) == false)
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
        else if (playerDirection().y < 0 && playerDirection().y > -cc2d.bounds.extents.y && Mathf.Abs(playerDirection().x) < 2f /*Vector3.Distance(GameObject.Find("Player").transform.position, transform.position) < 2f*/)
        {
            /*if (playerDirection().x < 0)
                direction = -1;
            else if (playerDirection().x > 0)
                direction = 1;*/

            //Debug.Log("Player in der Nähe");

            //Attack();
            //Debug.Log("attacke passiert?");

            //moveSpeed = attackSpeed;
            //if (!anim.GetBool("isAttacking"))
                //AudioController.Instance.playFXSound(oniAttack);

            //anim.SetBool("isAttacking", true);
            //anim.SetBool("isSliding", false);
        }

        if (isSliding)
        {
            isIdle = false;
            isJumping = false;
            isFalling = false;
            Slide();
            //Debug.Log("Kappa slided");
        }
        else if (isIdle)
        {
            currentIdleTime += Time.deltaTime;

            if(currentIdleTime >= idleTime)
            {
                currentIdleTime = 0;
                //isFacingRight = !isFacingRight;
                //Jump();
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

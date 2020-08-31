using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioControlling;

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
    public bool jumpStart = false;
    public float jumpDistance;

    public Audio kappaJump;
    public Audio kappaHit;

    private Animator anim;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lastYPos = transform.position.y;
        attackRadius = 1.3f;
        anim = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();
        Debug.DrawRay(transform.position, velocity);
        //Debug.Log("position: " + transform.position);
        //Debug.Log("Velocity: " + velocity);
        //Debug.Log("playerdirection: " + playerDirection().normalized);
        Debug.Log("isSliding? " + isSliding);
        Debug.Log("isGrounded? " + grounded);

        //patrol
        if (isSliding)
        {
            isIdle = false;
            isJumping = false;
            isFalling = false;
            Slide();
            //Debug.Log("Kappa slided");
        }

        if (isIdle && playerDirection().x < 20f && !isSliding)
        {
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

            anim.SetBool("isJumping", false);
            anim.SetBool("isIdle", true);
            jumpStart = true;
        }
        else if(transform.position.y > lastYPos && grounded == false && isIdle == false)
        {
            isJumping = true;
            isFalling = false;

            anim.SetBool("isJumping", true);
            anim.SetBool("jumpStart", jumpStart);
            jumpStart = false;
        }
        else if(transform.position.y < lastYPos && grounded == false && isIdle == false)
        {
            isJumping = false;
            isFalling = true;
        }

        lastYPos = transform.position.y;

        airMovement();
    }

    protected override void Jump()
    {
        
        isSliding = false;
        isIdle = false;
        isJumping = true;

            if(playerDirection().x < 0f)
            {
                direction = -1f;
                CharacterFacingDirection(direction);
            }
            else
            {
                direction = 1f;
                CharacterFacingDirection(direction);
            }

            velocity = new Vector2(playerDirection().normalized.x * jumpDistance, jumpHeight);
        AudioController.Instance.playFXSound(kappaJump);
    }

    void airMovement()
    {
        //Debug.Log("Air movement wird aufgerufen");
        if (!grounded && direction != 0 && velocity.magnitude < maxAirMovementSpeed && !isSliding)
        {
            velocity += (direction * moveWhileJumping) * Vector2.right * (1 - wallJumpTime) * (1 / ((0.1f + Mathf.Abs(velocity.x) * 0.5f))); //velocity = new Vector2((velocity.x + (moveWhileJumping * moveDirection)) * Mathf.Pow(airResistance, velocity.magnitude) * (1 - wallJumpTime), velocity.y);
            isSliding = false;
            //Debug.Log("Airmovement funktioniert?");
        }
    }
}

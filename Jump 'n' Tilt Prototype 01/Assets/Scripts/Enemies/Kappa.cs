using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioControlling;

// Author: Nicole Mynarek
public class Kappa : GroundEnemy
{
    protected float lastYPos = 0;               //last y position of Kappa
    public float idleTime = 0.2f;               //time in which the Kappa does not jump
    protected float currentIdleTime = 0;        //current time, when the kappa does not jump yet
    protected bool isIdle = true;               //true if Kappa sits on ground
    protected bool isJumping = false;           //true if Kappa jumps
    protected bool isFalling = false;           //true if Kappa falls down
    protected bool jumpStart = false;           
    protected bool slideStart = false;
    public float jumpDistance;                  //length of Kappa's jump
    public float distanceToPlayer;              //distance between Kappa and player

    protected BoxCollider2D bc2d;

    public Audio kappaJump;
    public Audio kappaHit, kappaBlock;

    public GameObject dust;

    protected override void Start()
    {
        base.Start();
        lastYPos = transform.position.y;
        attackRadius = 1.2f;
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        attackPos.position = new Vector3(bc2d.bounds.center.x, bc2d.bounds.center.y, 0f);
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        //Kappa slides if isSliding is true
        if (isSliding)
        {
            isIdle = false;
            isJumping = false;
            isFalling = false;
            Slide();

            //slide animation
            anim.SetBool("isSliding", true);
            anim.SetBool("slideStart", slideStart);
            slideStart = false;
        }
        else
        {
            anim.SetBool("isSliding", false);
            slideStart = true;
        }

        //Kappa starts jumping if he sits on ground and the level is not tilted and player falls below a spedific distance
        if (isIdle && Mathf.Abs(playerDirection().x) < distanceToPlayer && !isSliding)
        {
            currentIdleTime += Time.deltaTime;

            //Kappa jumps only after a specific time
            if(currentIdleTime >= idleTime)
            {
                currentIdleTime = 0;
                Jump();
            }
        }

        //Animation handling
        if(grounded == true && isJumping == false)
        {
            isIdle = true;
            isJumping = false;
            isFalling = false;

            attackPos.position = new Vector3(bc2d.bounds.center.x, bc2d.bounds.center.y, 0f);

            //start to jump animation
            anim.SetBool("isJumping", false);
            anim.SetBool("isIdle", true);
            jumpStart = true;
        }
        else if(transform.position.y > lastYPos && grounded == false && isIdle == false)
        {
            //jump animation
            isJumping = true;
            isFalling = false;

            if (isFacingRight)
            {
                attackPos.position = new Vector3(bc2d.bounds.center.x + 0.6f, bc2d.bounds.center.y + 1.1f, 0f);
            }
            else
            {
                attackPos.position = new Vector3(bc2d.bounds.center.x - 0.6f, bc2d.bounds.center.y + 1.1f, 0f);
            }
             //fall animation
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

        //to stop Kappas movement in the air, the airMovement function is called only if player is alive
        if (player.GetComponent<PlayerCharacter>().health > 0)
        {
            airMovement();
        }
    }

    //overridden jump function
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
        AudioController.Instance.playFXSound(kappaJump);

        //jumping at one position when player s dead
        if (player.GetComponent<PlayerCharacter>().health == 0)
        {
            velocity = new Vector2(0f, jumpHeight);
        }
        //jumping in players direction
        else
        {
            velocity = new Vector2(playerDirection().normalized.x * jumpDistance, jumpHeight);
        }
    }

    //method to be sure that Kappa continues to move in air in front of a platform, so he can land on it
    protected void airMovement()
    {
        if (!grounded && direction != 0 && velocity.magnitude < maxAirMovementSpeed && !isSliding)
        {
            velocity += (direction * moveWhileJumping) * Vector2.right * (1 - wallJumpTime) * (1 / ((0.1f + Mathf.Abs(velocity.x) * 0.5f))); //velocity = new Vector2((velocity.x + (moveWhileJumping * moveDirection)) * Mathf.Pow(airResistance, velocity.magnitude) * (1 - wallJumpTime), velocity.y);
            isSliding = false;
        }
    }

    //overridden method to block damage if Kappa does not face the player
    //not working correctly
    public override void TakeDamage(int damage, Vector2 direction)
    {
        //Kappa gets hit when facing player
        if (direction.x < 0  && !isFacingRight || direction.x > 0 && isFacingRight || isSliding)
        {
            AudioController.Instance.playFXSound(kappaHit);
            base.TakeDamage(damage, direction);
        }
        //Kappa blocks when not facing the player
        else
        {
            Instantiate(dust, transform.position, Quaternion.identity);
            AudioController.Instance.playFXSound(kappaBlock);
        }
    }
}

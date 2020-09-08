using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioControlling;

// Author: Nicole Mynarek
public class Kappa : GroundEnemy
{
    protected float lastYPos = 0;
    public float idleTime = 0.2f;
    protected float currentIdleTime = 0;
    protected bool isIdle = true;
    protected bool isJumping = false;
    protected bool isFalling = false;
    protected bool jumpStart = false;
    protected bool slideStart = false;
    public float jumpDistance;
    public float distanceToPlayer;

    protected BoxCollider2D bc2d;

    public Audio kappaJump;
    public Audio kappaHit, kappaBlock;

    public GameObject dust;

    private Animator anim;

    // Start is called before the first frame update
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

        //patrol
        if (isSliding)
        {
            isIdle = false;
            isJumping = false;
            isFalling = false;
            Slide();

            anim.SetBool("isSliding", true);
            anim.SetBool("slideStart", slideStart);
            slideStart = false;
        }
        else
        {
            anim.SetBool("isSliding", false);
            slideStart = true;
        }

        if (isIdle && Mathf.Abs(playerDirection().x) < distanceToPlayer && !isSliding && GameObject.Find("Player").GetComponent<PlayerCharacter>().health >= 0)
        {
            currentIdleTime += Time.deltaTime;

            if(currentIdleTime >= idleTime)
            {
                currentIdleTime = 0;
                Jump();
            }
        }
        else if(isIdle && playerDirection().x < distanceToPlayer && !isSliding && GameObject.Find("Player").GetComponent<PlayerCharacter>().health == 0)
        {
            currentIdleTime += Time.deltaTime;

            if (currentIdleTime >= idleTime)
            {
                currentIdleTime = 0;
                JumpWin();
            }
        }

        if(grounded == true && isJumping == false)
        {
            isIdle = true;
            isJumping = false;
            isFalling = false;

            attackPos.position = new Vector3(bc2d.bounds.center.x, bc2d.bounds.center.y, 0f);

            anim.SetBool("isJumping", false);
            anim.SetBool("isIdle", true);
            jumpStart = true;
        }
        else if(transform.position.y > lastYPos && grounded == false && isIdle == false)
        {
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

        if (GameObject.Find("Player").GetComponent<PlayerCharacter>().health >= 0)
        {
            airMovement();
        }
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
        AudioController.Instance.playFXSound(kappaJump);
        velocity = new Vector2(playerDirection().normalized.x * jumpDistance, jumpHeight);
        
        }

    protected void JumpWin()
    {

        isSliding = false;
        isIdle = false;
        isJumping = true;

        if (playerDirection().x < 0f)
        {
            direction = -1f;
            CharacterFacingDirection(direction);
        }
        else
        {
            direction = 1f;
            CharacterFacingDirection(direction);
        }
        
        //AudioController.Instance.playFXSound(kappaJump);
        velocity = new Vector2(0f, jumpHeight);

        Debug.Log("Player ist tot");
    }

    protected void airMovement()
    {
        if (!grounded && direction != 0 && velocity.magnitude < maxAirMovementSpeed && !isSliding)
        {
            velocity += (direction * moveWhileJumping) * Vector2.right * (1 - wallJumpTime) * (1 / ((0.1f + Mathf.Abs(velocity.x) * 0.5f))); //velocity = new Vector2((velocity.x + (moveWhileJumping * moveDirection)) * Mathf.Pow(airResistance, velocity.magnitude) * (1 - wallJumpTime), velocity.y);
            isSliding = false;
        }
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        if (direction.x < 0  && !isFacingRight || direction.x > 0 && isFacingRight || isSliding)
        {
            Debug.Log("player direction: " + playerDirection().x);
            AudioController.Instance.playFXSound(kappaHit);
            base.TakeDamage(damage, -direction);
        }
        else
        {
            Instantiate(dust, transform.position, Quaternion.identity);
            AudioController.Instance.playFXSound(kappaBlock);
        }

        Debug.Log("damage direction: " + direction);
        Debug.Log("isSliding? " + isSliding);
    }

    /*void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(attackPos.position, attackRadius);
    }*/
}

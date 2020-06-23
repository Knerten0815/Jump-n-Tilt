using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PhysicsObject
{

    protected float moveDirection;              // Gets value between -1 and 1 for direction left or right if Input.GetAxisRaw is used

    // Values can be adjusted in inspector
    public float moveSpeed;                // Movement Speed
    public float jumpHeight;               //Gravity Modifier in inspector is set to 4 to get a non floaty feeling
    public float jumpHeightReducer;      // Reduces jump height, if button is pressed shortly
    public float moveWhileJumping;         // Movement value while jumping
    public float airResistance;              //1 no resistance, 0 no movement possible
    protected float wallJumpTime;             //by Marvin Winkler, used so that midAir movement does not overreide the wall jump

    public float slideSpeed;
    public float slideReducer;

    private Vector2 slideDirection;

    public bool isFacingRight;
    public bool isSliding;

    // for Attack method
    public Transform attackPos;                 // is set in Unity window
    public float attackRadius;
    public LayerMask whatIsEnemy;
    public int health;

    // inherited from PhysicsObject.cs
    protected override void OnEnable()
    {
        base.OnEnable();
        wallJumpTime = 0;
    }

    // Author: Michelle Limbach, Nicole Mynarek, Marvin Winkler
    protected override void ComputeVelocity()
    {
        // Player only slides when there is no input
        if (moveDirection != 0)
        {
            // character looks in the direction he is moving
            CharacterFacingDirection(moveDirection);
        }
        else
        {
            Slide();
        }
        moveDirection = 0;

        // death of character
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if(wallJumpTime < 0)
        {
            wallJumpTime = 0;
        }
        else if(wallJumpTime > 0)
        {
            wallJumpTime -= Time.deltaTime;
        }
    }

    // Author: Nicole Mynarek, Michelle Limbach, Marvin Winkler
    // Method for basic horizontal movement 
    protected virtual void Movement(float direction)
    {
        moveDirection = direction;

        if (grounded)
        {
            // if slideDirection and moveDirection are both negativ or positiv, then the player moves faster
            if (slideDirection.x < 0 && moveDirection < 0 || slideDirection.x > 0 && moveDirection > 0)
            {
                velocity = new Vector2(moveDirection * (moveSpeed + slideSpeed), velocity.y);
            }
            // if slideDirection and moveDirection have unequal signs (e. g. one is positive and the other one is negative), then the player moves slower
            else if (slideDirection.x < 0 && moveDirection > 0 || slideDirection.x > 0 && moveDirection < 0)
            {
                velocity = new Vector2(moveDirection * (moveSpeed + slideReducer), velocity.y);
            }
            else
            {
                velocity = new Vector2(moveDirection * moveSpeed, velocity.y);
            }
        }
        // if player is in the air and gives input, the player can move left or right
        else if (!grounded && moveDirection != 0 && wallJumpTime == 0)
        {
            velocity = new Vector2((velocity.x + (moveWhileJumping * moveDirection)) * Mathf.Pow(airResistance, velocity.magnitude), velocity.y);
        }
    }

    // Author: Nicole Mynarek
    // Method for basic jump
    protected virtual void Jump()
    {
        if (jumpable)
        {
            // Gravity Modifier of PhysicsObject.class needs to be adjusted according to jumpHeight for good game feeling
            velocity = new Vector2(velocity.x, jumpHeight);
        }
    }

    // Author: Nicole Mynarek, Rewritten for debugging by Marvin Winkler
    // Method for flipping character sprites according to moving direction
    protected void CharacterFacingDirection(float direction)
    {
        if(direction < 0)
        {
            isFacingRight = false;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(direction > 0)
        {
            isFacingRight = true;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    // Author: Nicole Mynarek, Michelle Limbach, Marvin Winkler
    // Method for sliding. Player always looks in direction of the tilt. 
    // Variable 'minGroundNormalY' from PhysicsObject.class can be adjusted for different results
    // Sliding speed still has to be adjusted
    protected virtual void Slide()
    {
        Vector2 normal;

        if (grounded)
        {
            if (groundNormal != new Vector2(0f, 1f))
            {
                isSliding = true;

                for (int i = 0; i < hitBufferList.Count; i++)
                {
                    normal = hitBufferList[i].normal;
                   
                    // left tilt direction
                    if (normal.x < 0)
                    {
                        slideDirection = Vector2.Perpendicular(normal);
                        float temp = 1f + slideDirection.x; 
                        slideDirection.x = -1 - temp; 
                        CharacterFacingDirection(slideDirection.x);
                    }
                    // right tilt direction
                    else
                    {
                        slideDirection = Vector2.Perpendicular(-normal);
                        float temp = 1f - slideDirection.x;
                        slideDirection.x = 1 + temp;
                        CharacterFacingDirection(slideDirection.x);
                    }
                }
                if(velocity.x <= 10 && velocity.x >= -10)
                {
                    velocity += slideDirection;
                } 
            }
            else
            {
                isSliding = false;
                slideDirection = new Vector2(0f, 0f);
            }
        }
    }

    protected virtual void Attack()
    {
        //  Debug.Log("Nicole ---------- ATTACK!!!!!!!");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Enemy>().TakeDamage(1);
        }
    }

    protected virtual void TakeDamage(int damage)
    {

    }
}

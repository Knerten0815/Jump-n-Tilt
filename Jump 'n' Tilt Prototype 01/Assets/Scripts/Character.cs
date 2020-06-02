using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PhysicsObject
{

    protected float moveDirection;              // Gets value between -1 and 1 for direction left or right if Input.GetAxisRaw is used

    // Values can be adjusted in inspector
    public float moveSpeed = 10f;                // Movement Speed
    public float jumpHeight = 16f;
    public float jumpHeightReducer = 0.5f;      // Reduces jump height, if button is pressed shortly
    public float moveWhileJumping = 7f;         // Movement value while jumping

    public float slideSpeed = 2f;
    public float slideReducer = 0.5f;

    private Vector2 slideDirection;

    public bool isFacingRight = true;
    public bool isSliding;

    // for Attack method
    public Transform attackPos;
    public float attackRadius;
    public LayerMask whatIsEnemy;
    public int health;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    // Author: Michelle Limbach, Nicole Mynarek
    protected override void ComputeVelocity()
    {
        // Player only slides when there is no input
        if (moveDirection != 0)
        {
            CharacterFacingDirection(moveDirection);

        }
        else
        {
            Slide();
        }
        moveDirection = 0;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Author: Nicole Mynarek, Michelle Limbach
    // Method for basic horizontal movement 
    protected virtual void Movement(float direction)
    {
        moveDirection = direction;
        
        if (grounded)
        {
            // if slideDirection and moveDirection are both negativ or positiv, then the playere moves faster
            if (slideDirection.x < 0 && moveDirection < 0 || slideDirection.x > 0 && moveDirection > 0)
            {
                velocity = new Vector2(moveDirection * moveSpeed * slideSpeed, velocity.y);
        
            }
            // if slideDirection and moveDirection have unequal signs (e. g. one is positive and the other one is negative), then the player moves slower
            else if (slideDirection.x < 0 && moveDirection > 0 || slideDirection.x > 0 && moveDirection < 0)
            {
                velocity = new Vector2(moveDirection * moveSpeed * slideReducer, velocity.y);
         
            }
            else
            {
                velocity = new Vector2(moveDirection * moveSpeed, velocity.y);
            
            }
        }
        // if player is in the air and gives input, the player can move left or right
        else if (!grounded && moveDirection != 0)
        {
            velocity = new Vector2(moveWhileJumping * moveDirection, velocity.y);
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

    // Author: Nicole Mynarek
    // Method for flipping character sprites according to moving direction
    void CharacterFacingDirection(float direction)
    {
        if (isFacingRight && direction < 0)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (!isFacingRight && direction > 0)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    // Author: Nicole Mynarek, Michelle Limbach
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
                   
                    if (normal.x < 0)
                    {
                        slideDirection = Vector2.Perpendicular(normal);
                        float temp = 1f + slideDirection.x;
                        slideDirection.x = -1 - temp*2;
                        CharacterFacingDirection(slideDirection.x);
                    }
                    else
                    {
                        
                        slideDirection = Vector2.Perpendicular(-normal);
                        float temp = 1f - slideDirection.x;
                        slideDirection.x = 1 + temp*2;
                        CharacterFacingDirection(slideDirection.x);
                    }
                }
                velocity += slideDirection;
                
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

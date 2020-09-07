using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PhysicsObject
{

    protected float moveDirection;              // Gets value between -1 and 1 for direction left or right if Input.GetAxisRaw is used

    // Values can be adjusted in inspector
    public float moveSpeed;                 //Movement Speed
    public float jumpHeight;                //Height of normal jumps
    public float jumpHeightReducer;         //Reduces jump height, when letting go of the jump button
    public float moveWhileJumping;          //Movement speed while in air
    public float airResistance;             //by Marvin Winkler, 1 no resistance, 0 no movement possible (x-axis dampening wile in the air)
    public float wallJumpTime;              //by Marvin Winkler, used so that midAir movement does not overreide the wall jump
    protected float wallJumpTimer;          //by Marvin Winkler, used so that midAir movement does not overreide the wall jump
    public float maxAirMovementSpeed;       //by Marvin Winkler, maximum horizontal air movement speed

    public bool onWall;                     //by Marvin Winkler, used to fix wall climbing while level is tilted, is true if level is tilted and player is touching the wall
    public bool isFacingRight;

    //Slide
    public float slideSpeed;                //slide accelleration, not speed
    public float slideBackwardsMaxSpeed;    //by Marvin Winkler, max speed while pressing against the tilt
    public Vector2 slideDirection;          //by Marvin Winkler, current slide direction, only acurate when sliding 
    protected bool isSliding;               //by Marvin Winkler, true while sliding
    public float slideDampeningFactor;      //by Marvin Winkler, dampening factor used while sliding on ground

    // for Attack method
    public Transform attackPos;             // is set in Unity window
    public float attackRadius;
    public LayerMask whatIsEnemy;

    // taking Damage, getting knocked back
    public int health;
    public float knockback;                 //by Marvin Winkler, backwards velocity given to character when hit
    public float knockup;                   //by Marvin Winkler, upwards velocity given to character when hit
    protected bool isDead;                  //by Marvin Winkler, true when character is dead

    public GameObject bloodSpray;

    // inherited from PhysicsObject.cs
    protected override void OnEnable()
    {
        base.OnEnable();
        isDead = false;
    }

    // Author: Michelle Limbach, Nicole Mynarek, Marvin Winkler
    //gets called onece per update
    protected override void ComputeVelocity()
    {
        // Player only slides when there is no input
        if (moveDirection != 0)
        {
            // character looks in the direction he is moving
            CharacterFacingDirection(moveDirection);
        }

    }

    // Author: Nicole Mynarek, Michelle Limbach, rewritten for debugging by Marvin Winkler
    // Method for basic horizontal movement 
    protected virtual void Movement(float direction)
    {
        moveDirection = direction;

        if (onWall)
        {
            return; // this disables manual movement if the player is on a wall while the level is tilted, thus disabling wall climbing
        }

        // if player is in the air and gives input, the player can move left or right
        if (!grounded && velocity.magnitude < maxAirMovementSpeed && !isSliding || wallJumpTimer > 0)
        {
            velocity += (moveDirection * moveWhileJumping) * Vector2.right * (1 / ((0.1f + Mathf.Abs(velocity.x) * 0.5f)));
        }

        //Ground based movement
        //Here velocity gets a new vector, therefore the speed/direction change happens instantly, there is no excelleration time
        else if (!isSliding)
        {
            velocity = new Vector2(moveDirection * moveSpeed, velocity.y);
        }
    }

    // Author: Nicole Mynarek, Marvin Winkler
    // Method for basic jump
    protected virtual void Jump()
    {
        if (jumpable)
        {
            isSliding = false;
            // Gravity Modifier of PhysicsObject.class needs to be adjusted according to jumpHeight for good game feeling
            velocity = new Vector2(Input.GetAxis("Horizontal") * maxAirMovementSpeed, jumpHeight);
        }
    }

    // Author: Nicole Mynarek, rewritten for debugging by Marvin Winkler
    // Method for flipping character sprites according to moving direction
    protected virtual void CharacterFacingDirection(float direction)
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
    // Sliding speed still has to be adjusted
    protected virtual void Slide()
    {
         Vector2 normal;

        if (grounded)
        {
            if (isSliding)
            {

                for (int i = 0; i < hitBufferList.Count; i++)
                {
                    normal = hitBufferList[i].normal;
                   
                    // left tilt direction
                    if (normal.x < 0)
                    {
                        slideDirection.x = -1;
                        CharacterFacingDirection(slideDirection.x);
                    }
                    // right tilt direction
                    else
                    {
                         slideDirection.x = 1;
                        CharacterFacingDirection(slideDirection.x);
                    }
                }

                //add velocity
                if(velocity.x <= maxSpeed && velocity.x >= -maxSpeed)
                {
                    velocity += slideDirection * slideSpeed;
                } 
            }
        }
    }

    //Special dampening while sliding, base dampening otherwise
    //Author: Marvin Winkler
    protected override void calculateDampening()
    {
        if (!isSliding)
        {
            base.calculateDampening();
        }
        else
        {
            if (grounded)
            {
                if (timeController.getTimeSpeed() < 1)
                {
                    velocity *= (slideDampeningFactor + (1 - dampening) * timeController.getTimeSpeed());
                }
                else
                {
                    velocity *= slideDampeningFactor;
                }
            }
        }
    }
    
    //Damages all enemies of the character within the attack radius of the attack position
    //Author: Nicole Mynarek, Marvin Winkler
    protected virtual void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);

        for (int i = 0; i < enemies.Length; i++)
        {

            Vector3 dmgDirection = gameObject.transform.localPosition - enemies[i].GetComponent<Transform>().localPosition;
            Vector2 dmgDirection2D = new Vector2(dmgDirection.x, dmgDirection.y);
            dmgDirection2D.Normalize();
            Character enemy = enemies[i].gameObject.GetComponent<Character>();
            if (enemy != null)
                enemies[i].GetComponent<Character>().TakeDamage(1, dmgDirection2D);
        }
    }

    // Author: Nicole Mynarek, Marvin Winkler
    // gets called when character was attacked and hit
    // damages character and knocks him back
    public virtual void TakeDamage(int damage, Vector2 direction)
    {
        health -= damage;
        velocity = new Vector2(-direction.x * knockback, knockup);
        CharacterFacingDirection(-velocity.x);
        if(health <= 0)
        {
            isDead = true;
        }
    }
}

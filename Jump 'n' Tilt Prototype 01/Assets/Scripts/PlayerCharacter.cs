using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;

public class PlayerCharacter : Character
{

    // Author: Nicole Mynarek

    // variables for jumping
    public int jumpCount = 2;           // Possible amount of jumps
    public int jumpCountLeft;           // Amount of jumps that are left
    public float cooldown = 0.8f;
    private bool canJump = true; //Is Player allowed to jump

    // variables for crouching
    private bool crouching = false; //Is player crouching
    private bool underPlattform; //Checks if there is a plattform over the head of the player
    private float plattformCheckDistance = 1f; //Distance for Raycast is set to 15, because it is the half of the Player size
    private bool inFrontOfPlayer;
    private bool behindPlayer;

    // variables for wall jump, wall sliding and detection
    private RaycastHit2D lastWallcontact;
    public int wallJumpCounter = 2;
    public bool touchesWall;    // for wall detection
    public float wallCheckDistance = 0.5f;  //public Vector2 offsetRight = new Vector2(0.5f, 0);
    public LayerMask whatIsLevel;    //public Transform test;
    public bool wallSliding;
    public float wallSlidingSpeed = 0.1f;
    public int facingDirection = 1; // has to be set to 1 because isFacingRight is set to true. Maybe needs to be in CharacterClass?
    private RaycastHit2D hit;

    protected override void OnEnable()
    {
        base.OnEnable();

        // Nicole 
        PlayerInput.onHorizontalDown += Movement;
        PlayerInput.onJumpButtonDown += Jump;
        PlayerInput.onJumpButtonUp += ShortJump;
        PlayerInput.onPlayerAttackDown += Attack;

        //Michelle
        PlayerInput.onVerticalDown += CrouchDown;
        PlayerInput.onVerticalUp += CrouchUp;

        // Nicole 
        whatIsLevel = LayerMask.GetMask("Level");
        whatIsEnemy = LayerMask.GetMask("Enemy");
        
    }

    protected override void OnDisable()
    {
        // Nicole 
        PlayerInput.onHorizontalDown -= Movement;
        PlayerInput.onJumpButtonDown -= Jump;
        PlayerInput.onJumpButtonUp -= ShortJump;
        PlayerInput.onPlayerAttackDown -= Attack;

        //Michelle
        PlayerInput.onVerticalDown -= CrouchDown;
        PlayerInput.onVerticalUp -= CrouchUp;
    }

    // Author: Nicole Mynarek
    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        WallSliding();

    }

    protected override void Movement(float direction)
    {
        touchesWall = Physics2D.Raycast((Vector2)transform.position, transform.right, wallCheckDistance, whatIsLevel);
        base.Movement(direction);
    }

    private void WallSliding()
    {
        if (touchesWall && !grounded && velocity.y < 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            velocity = new Vector2(velocity.x, -wallSlidingSpeed);
        }
    }

    // Author: Nicole Mynarek
    // Method overridden, double jump is possible now
    protected override void Jump()
    {
        if (touchesWall)
        {
            hit = Physics2D.Raycast((Vector2)transform.position, transform.right, wallCheckDistance, whatIsLevel);
            WallJump();
        }
        else
        {
            if (grounded && velocity.y <= 0 && canJump)
            {
                if (cooldown <= 0)
                {
                    jumpCountLeft = jumpCount;
                    // Nicole: ------------- for coding cooldown is set to 0 --------------
                    cooldown = 0.8f;
                    //---------------------------------------------------------------------
                    base.Jump();
                    jumpCountLeft--;

                }

                //Wenn Player am Boden, dann reset vom WallJump
                wallJumpCounter = 2;
                lastWallcontact = new RaycastHit2D();
            }
            else if (jumpCountLeft <= 0)
            {
                jumpable = false;
            }
            else
            {
                jumpable = true;
                // Nicole: ----------- for coding cooldown set to 0 ----------------------
                cooldown = 0.8f;
                //------------------------------------------------------------------------
                base.Jump();
                jumpCountLeft--;

            }
        }
    }

    private void WallJump()
    {
        if (canJump && (lastWallcontact.point.x != hit.point.x || (wallJumpCounter > 0))) //Player springt ab
        {
            if (lastWallcontact.point.x != hit.point.x)
            {
                wallJumpCounter = 2;
            }
            wallSliding = false;
            jumpCountLeft--;
            velocity = new Vector2(moveSpeed * moveDirection, jumpHeight);
            lastWallcontact = hit;
            wallJumpCounter--;

        }
        else
        {
            WallSliding();
            jumpCountLeft = 0;
        }
    }

    private void ShortJump()
    {
        //If von Michelle hinzugefügt
        if (canJump)
        {
            velocity = new Vector2(velocity.x, velocity.y * jumpHeightReducer);
        }
    }

    //Author: Michelle Limbach
    private void CrouchDown(float direction)
    {
        //Player is not crouching yet, is grounded and the Arrow down Button is pressed
        if (!crouching && grounded && direction < 0)
        {
            //Scale the Sprite
            GetComponent<SpriteRenderer>().size = GetComponent<SpriteRenderer>().size * new Vector2(1f, 0.5f);

            //Scale the CapsuleCollider and set with offset to new position, so player does not get stuck in ground
            GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, 15.44461f);
            GetComponent<CapsuleCollider2D>().offset = new Vector2(1.019688f, -0.01133485f);

            //Set crouching to true, so Script news player is already crouching
            crouching = true;

            //Decrease movement speed
            moveSpeed = moveSpeed - 3f;

            //Player cannot jump while crouching
            canJump = false;

        }

    }

    // Author: Michelle Limbach
    private void CrouchUp(float direction)
    {
        //If the player is crouching
        if (crouching)
        {
            //Checks if Player has a Plattform over his head
            underPlattform = Physics2D.Raycast((Vector2)transform.position, transform.up, plattformCheckDistance, whatIsLevel);

            //If there is no Plattform above the Player
            if (!underPlattform)
            {
                //Make a new Raycast behind the Player to ensure that the Player is far enough out of the cave

                inFrontOfPlayer = Physics2D.Raycast((Vector2)transform.position + new Vector2(0.3f, 0f), transform.up, plattformCheckDistance, whatIsLevel);
                behindPlayer = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.3f, 0f), transform.up, plattformCheckDistance, whatIsLevel);


                // If there is no Plattfrom over or right behind the Player, the Player can stand up
                if (!inFrontOfPlayer && !behindPlayer)
                {
                    //Rescale the Sprite
                    GetComponent<SpriteRenderer>().size = GetComponent<SpriteRenderer>().size * new Vector2(1f, 2f);

                    //Rescale the CapsuleCollider size and offset and put the player in a higher y position, so the player does not get stuck in ground 
                    transform.position += new Vector3(0f, 0.3f, 0f);
                    GetComponent<CapsuleCollider2D>().size = new Vector2(GetComponent<CapsuleCollider2D>().size.x, 27.17294f);
                    GetComponent<CapsuleCollider2D>().offset = new Vector2(1.019688f, 0.2658937f);

                    //Set crouching to false, because the player does not crouch anymore
                    crouching = false;

                    //Increase movement speed
                    moveSpeed = moveSpeed + 3f;

                    canJump = true;
                }
            }

        }

    }

    /*
    *
    * Checks for collision with a pickup object.
    * 
    * @Katja
    *
    */
    protected void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PickUp", if it is...
        //Doing it like this can be costly if the function returns null, if we ever have trouble performance here it could be optimized
        PickUp pickUpComponent = other.gameObject.GetComponent<PickUp>();
        if (pickUpComponent != null)
        {
            other.gameObject.SetActive(false);
            pickUpComponent.hit();
        }
    }

    }

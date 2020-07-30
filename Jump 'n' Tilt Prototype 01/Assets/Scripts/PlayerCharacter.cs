using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;

public class PlayerCharacter : Character
{

    // Author: Nicole Mynarek, Michelle Limbach

    // variables for jumping
    public int jumpCount;                   // Possible amount of jumps
    public int jumpCountLeft;                   // Amount of jumps that are left
    private float cooldown;
    public float jumpCooldownTime;
    private bool canJump = true;                //Is Player allowed to jump

    // variables for crouching
    private bool crouching = false;             //Is player crouching
    private bool underPlattform;                //Checks if there is a plattform over the head of the player
    private float plattformCheckDistance = 1f;  //Distance for Raycast is set to 15, because it is the half of the Player size
    private bool inFrontOfPlayer;
    private bool behindPlayer;

    // variables for wall jump, wall sliding and detection
    private RaycastHit2D lastWallcontact;
    public int wallJumpCounter;
    public bool touchesWall;                    // for wall detection
    public float wallCheckDistance;      //public Vector2 offsetRight = new Vector2(0.5f, 0);
    public LayerMask whatIsLevel;               //public Transform test;
    public bool wallSliding;
    public float wallSlidingSpeed;       // can be adjusted in inspector for finding better setting
    public int facingDirection;             // has to be set to 1 because isFacingRight is set to true. Maybe needs to be in CharacterClass?
    private RaycastHit2D hit;
    private static float wallJumpTimer = 1;             //by Marvin Winkler, determines how long mid air movemnet is disabled after a wall jump
    public float wallJumpSpeed;             //by Marvin Winkler, speed given to the player when jumping of a wall
    private LevelControlls.LevelControllerNew levelController; //by Marvin Winkler, used to fix wall climbing bug while level is tilted

    protected override void OnEnable()
    {
        base.OnEnable();

        //Marvin
        levelController = GameObject.Find("LevelController").GetComponent<LevelControlls.LevelControllerNew>();

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

    // Author: Nicole Mynarek, Marvin Winkler
    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        // jump cooldown
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        WallSliding();

        onWall = false;

        if (touchesWall && levelController.getTiltStep() != 0)
        {
            onWall = true;
        }
        Debug.Log(onWall);
    }

    protected override void Movement(float direction)
    {
            base.Movement(direction);
    }

    // Author: Nicole Mynarek, Marvin Winkler
    private void WallSliding()
    {
        // checking if player touches wall (for wallSliding, wallJump), touchesWall is a bool
        touchesWall = (Physics2D.Raycast((Vector2)transform.position, transform.right, wallCheckDistance, whatIsLevel) || Physics2D.Raycast((Vector2)transform.position, -transform.right, wallCheckDistance, whatIsLevel));  
        // if player touches wall and is in air, wallSliding is true
        if (touchesWall && !grounded && velocity.y < 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        // if wallSliding is true, player slides down
        if (wallSliding)
        {
            velocity = new Vector2(velocity.x, -wallSlidingSpeed);
        }
    }

    // Author: Nicole Mynarek, Michelle Limbach, Marvin Winkler fixed bugges and removed hardcoded values and replaced them with variables
    // Method overridden, double jump is possible now
    protected override void Jump()
    {
        // if touchesWall is true, player can do a wallJump
        if (wallSliding)
        {
            hit = Physics2D.Raycast((Vector2)transform.position, transform.right, wallCheckDistance, whatIsLevel);
            lastWallcontact = Physics2D.Raycast((Vector2)transform.position, -transform.right, wallCheckDistance, whatIsLevel);
            if (hit.distance < lastWallcontact.distance)
            {
                hit = lastWallcontact;
            }
            WallJump();
        }
        else
        {
            // if player is on the ground
            if (grounded && velocity.y <= 0 && canJump)
            {
                // and cooldown lower or equal to 0
                if (cooldown <= 0)
                {
                    // jumpCountLeft will be reset
                    jumpCountLeft = jumpCount;

                    // cooldown will be set
                    cooldown = jumpCooldownTime;
                    
                    base.Jump();

                    jumpCountLeft--;

                }

                // reset of wallJumpCounter
                wallJumpCounter = 2;
                lastWallcontact = new RaycastHit2D();
            }
            // if jumpCountLeft is lower than or equal to 0, player can not jump anymore
            else if (jumpCountLeft <= 0)
            {
                jumpable = false;
            }
            else
            {
                jumpable = true;

                // cooldown is set
                cooldown = jumpCooldownTime;
                base.Jump();
                jumpCountLeft--;

            }
        }
    }

    private void WallJump()
    {
        // player can jump if canJump is true and the location of the lastWallContact is different to the new contact location 'hit' 
        // or if the wallJumpCounter is higher than 0
        if (canJump && (lastWallcontact.point.x != hit.point.x || (wallJumpCounter > 0))) //Player springt ab
        {
            // if he location of the lastWallContact is different to the new contact location 'hit' 
            if (lastWallcontact.point.x != hit.point.x)
            {
                // wallJumpCounter is reset
                wallJumpCounter = 2;
            }
            
            wallSliding = false;
            jumpCountLeft--;
            velocity = new Vector2(hit.normal.x * wallJumpSpeed, jumpHeight); //moveSpeed * (moveDirection), jumpHeight);
            CharacterFacingDirection(hit.normal.x);
            jumpable = false;
            lastWallcontact = hit;
            wallJumpCounter--;
            wallJumpTime = wallJumpTimer;
        }
        else
        {
            WallSliding();
            jumpCountLeft = 0;
        }
    }

    // Author: Nicole Mynarek
    private void ShortJump()
    {
        // Michelle added: if condition
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

            //Set crouching to true, so Script knows player is already crouching
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

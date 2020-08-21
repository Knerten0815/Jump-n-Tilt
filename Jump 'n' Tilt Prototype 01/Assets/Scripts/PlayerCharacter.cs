using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;

public class PlayerCharacter : Character
{

    // Author: Nicole Mynarek, Michelle Limbach, Marvin Winkler

    // variables for jumping
    public int jumpCount;                   // Possible amount of jumps
    public int jumpCountLeft;                   // Amount of jumps that are left
    private float cooldown;
    public float jumpCooldownTime;
    private bool canJump = true;                //Is Player allowed to jump

    public float hangTime;                      //by Marvin Winkler, how late can the player jump after walking of a platform in seconds
    private float hangTimer;

    public float jumpBuffer;                    //by Marvin Winkler, how early can the player hit the jump button before hitting the ground in seconds
    private float jumpBufferTimer;

    // variables for crouching
    private bool crouching = false;             //Is player crouching
    private bool underPlattform;                //Checks if there is a plattform over the head of the player
    private float plattformCheckDistance = 1f;  //Distance for Raycast is set to 15, because it is the half of the Player size
    private bool inFrontOfPlayer;
    private bool behindPlayer;
    public float crouchSpeedModifier;           //Factor by which the players speed is adjusted while crouching

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

    private BoxCollider2D collider;

    // variables for animation by Marvin Winkler
    private Animator animator;
    private bool jumpStart;
    private float playerInputBuffer;
    private float fishTimer;
    private bool justTookDamage;
    public float stunnTime;
    private float stunnTimer;
    private float deadFishTimer;

    // particle stuff by Marvin Winkler
    private ParticleSystem footsteps;
    private ParticleSystem.EmissionModule footEmission;
    private ParticleSystem.MainModule footstepsMain;
    private float particleOffDelayTimer;
    private ParticleSystem groundImpact;
    private ParticleSystem.MainModule groundImpactMain;
    private bool justLanded;

    // attack stuff by Marvin Winkler
    private Transform fishTrans;
    public Vector3 attackOffset;            //Offset from player position where he attacks
    private float attackTimer;
    public float attackDelay;               //Minimum time between attacks in seconds
    private bool isAttacking;

    //pickup stuff by Marvin Winkler
    private bool hasTimePickup;
    private float sloMoTimer;
    public float sloMoTime;                 //Duration of SloMoTime power up in seconds

    public delegate void useSloMoTime();
    public static event useSloMoTime onUseSloMoTime;

    public delegate void fishCausedEarthquake(float playerInput);
    public static event fishCausedEarthquake onFishCausedEarthquake;

    public delegate void fishCausedEarthquakeStart(float playerInput);
    public static event fishCausedEarthquakeStart onFishCausedEarthquakeStart;


    protected override void OnEnable()
    {
        base.OnEnable();

        //Marvin
        levelController = GameObject.Find("LevelController").GetComponent<LevelControlls.LevelControllerNew>();
        fishTrans = GameObject.Find("Fish").GetComponent<Transform>();

        footsteps = GameObject.Find("Footsteps").GetComponent<ParticleSystem>();
        footEmission = footsteps.emission;
        footstepsMain = footsteps.main;
        groundImpact = GameObject.Find("FootLanding").GetComponent<ParticleSystem>();
        groundImpactMain = groundImpact.main;

        collider = GetComponent<BoxCollider2D>();

        animator = GetComponent<Animator>();
        jumpStart = true;
        justTookDamage = false;
        deadFishTimer = -101;
        sloMoTimer = -100;

        PlayerInput.onTiltDown += smashFishToTilt;
        PlayerInput.onTiltDown += disableSliding;
        PlayerInput.onSlowMoDown += useSloMoPickup;

        ManagementSystem.healthPickUpHit += addHealth;
        ManagementSystem.timePickUpHit += addTimePickup;

        //PlayerInput.onHorizontalDown += disableSliding;
        //PlayerInput.onJumpButtonDown += disableSliding;

        // Nicole 
        PlayerInput.onHorizontalDown += Movement;
        PlayerInput.onJumpButtonDown += Jump;
        PlayerInput.onJumpButtonUp += ShortJump;
        PlayerInput.onPlayerAttackDown += Attack;

        //Michelle
        PlayerInput.onVerticalDown += CrouchDown;
        PlayerInput.onVerticalUp += CrouchUp;

        // Nicole 
        //whatIsLevel = LayerMask.GetMask("Level");
        //whatIsEnemy = LayerMask.GetMask("Enemy");
        
    }
    //Author: Marvin Winkler
    //Used to fix several bugs
    private void disableSliding(float a)
    {
        isSliding = false;
    }
    //Author: Marvin Winkler
    //Used to fix several bugs
    private void disableSliding()
    {
        isSliding = false;
    }

    protected override void OnDisable()
    {
        disableInput();
    }

    private void disableInput()
    {
        // Nicole 
        PlayerInput.onHorizontalDown -= Movement;
        PlayerInput.onJumpButtonDown -= Jump;
        PlayerInput.onJumpButtonUp -= ShortJump;
        PlayerInput.onPlayerAttackDown -= Attack;

        //Michelle
        PlayerInput.onVerticalDown -= CrouchDown;
        PlayerInput.onVerticalUp -= CrouchUp;

        //Marvin
        ManagementSystem.healthPickUpHit -= addHealth;
        ManagementSystem.timePickUpHit -= addTimePickup;
        PlayerInput.onTiltDown -= smashFishToTilt;
        PlayerInput.onTiltDown -= disableSliding;
        PlayerInput.onSlowMoDown -= useSloMoPickup;
        //PlayerInput.onHorizontalDown -= disableSliding;
        //PlayerInput.onJumpButtonDown -= disableSliding;
    }

    // Author: Nicole Mynarek, Marvin Winkler
    protected override void ComputeVelocity()
    {
        if(!isDead)
            base.ComputeVelocity();

        //hang time
        if (grounded)
        {
            hangTimer = hangTime;
        }
        else
        {
            hangTimer -= timeController.getSpeedAdjustedDeltaTime();
        }
        
        //jump buffer
        if(grounded && jumpBufferTimer > 0)
        {
            jumpBufferTimer = 0;
            jumpCountLeft = jumpCount;
            Jump();
        }
        else
        {
            jumpBufferTimer -= timeController.getSpeedAdjustedDeltaTime();
        }

        // jump cooldown
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        if (!isDead)
            WallSliding();

        onWall = false;

        if (touchesWall && levelController.getTiltStep() != 0)
        {
            onWall = true;
        }
        
        //SloMoTime powerup
        if(sloMoTimer >= 0)
        {
            sloMoTimer -= Time.deltaTime;
        }
        else if(sloMoTimer <= -100)
        {
            return;
        }
        else
        {
            onUseSloMoTime();
            sloMoTimer = -100;
        }

        //Particle system simulation speed
        footstepsMain.simulationSpeed = timeController.getTimeSpeed();
        groundImpactMain.simulationSpeed = timeController.getTimeSpeed();


        //Just for testing:
        //+++
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 hitDirectionTest = gameObject.transform.localPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 hitDirectionTest2D = new Vector2(hitDirectionTest.x, hitDirectionTest.y);
            hitDirectionTest2D.Normalize();
            TakeDamage(1, hitDirectionTest2D);
        }
        //+++

    }

    //Author: Marvin Winkler
    protected override void updateAnimations()
    {
        playAnimations();
        playParticleSystems();
    }

    //Author: Marvin Winkler
    //States for all the player animations
    private void playAnimations()
    {
        //Animation speed adjustment
        animator.speed = timeController.getTimeSpeed();

        //Is dead?
        animator.SetBool("isDead", isDead);
        //animator.SetBool("justDied", false);

        //Is running?
        animator.SetFloat("animationDirection", velocity.magnitude);

        //Is jumping?
        if (onWall || grounded)
        {
            animator.SetBool("isJumping", false);
            jumpStart = true;
        }
        else
        {
            animator.SetBool("isJumping", true);
            animator.SetBool("jumpStart", jumpStart);
            jumpStart = false;
        }

        animator.SetBool("IsJumpingUp", false);
        animator.SetBool("IsJumpingDown", false);

        if (!jumpable && velocity.y > 0)
        {
            animator.SetBool("IsJumpingUp", true);
        }
        else if (!jumpable && velocity.y < 0)
        {
            animator.SetBool("IsJumpingDown", true);
        }

        //Is on Wall?
        animator.SetBool("isOnWall", touchesWall);

        //Is crouching?
        animator.SetBool("isCrouching", crouching);

        //Is sliding?
        animator.SetBool("isSliding", isSliding);

        //Did level just tilt?
        if (fishTimer >= 0)
        {
            fishTimer -= timeController.getSpeedAdjustedDeltaTime();
        }
        else
        {
            if (fishTimer > -100)
            {
                onFishCausedEarthquake(playerInputBuffer);
                onFishCausedEarthquakeStart(0);
                fishTimer = -101;
            }
            else
            {
                onFishCausedEarthquake(Input.GetAxis("Tilt"));
            }
            animator.SetBool("justTilted", false);
        }

        //Did player just take Damage?
        if (justTookDamage)
        {
            animator.SetBool("justTookDamage", true);
            justTookDamage = false;
        }
        else
        {
            animator.SetBool("justTookDamage", false);
        }
        if(stunnTimer > 0)
        {
            stunnTimer -= timeController.getSpeedAdjustedDeltaTime();
            animator.SetBool("stunned", true);
        }
        else
        {
            animator.SetBool("stunned", false);
        }

        //Is the player attacking?
        if (attackTimer >= 0)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            attackTimer -= timeController.getSpeedAdjustedDeltaTime();
        }
        else
        {
            isAttacking = false;
            animator.SetBool("isAttacking", false);
        }

        //Is it time for the dead fish to spawn?
        //DON'T WRITE ANY NEW STUFF BELOW THIS IN playAnimations(), THE DEAD FISH STUFF NEEDS TO BE LAST!
        //+++
        if (deadFishTimer > 0)
        {
            deadFishTimer -= timeController.getSpeedAdjustedDeltaTime();
        }
        else if(deadFishTimer < -100)
        {
            return;
        }
        else if(deadFishTimer <= 0)
        {
            GameObject deadFish = GameObject.Find("deadFish");
            deadFish.GetComponent<SpriteRenderer>().enabled = true;
            deadFish.GetComponent<Animator>().enabled = true;

            bool isXFliped = gameObject.GetComponent<SpriteRenderer>().flipX;
            deadFish.GetComponent<SpriteRenderer>().flipX = isXFliped;

            if (isXFliped)
            {
                deadFish.GetComponent<Transform>().localPosition = new Vector3(15, 0, 1);
            }
            else
            {
                deadFish.GetComponent<Transform>().localPosition = new Vector3(-15, 0, 1);
            }

            deadFish.GetComponent<Animator>().speed = timeController.getTimeSpeed();
        }
        //+++
    }

    //Author: Marvin Winkler
    private void playParticleSystems()
    {
        if(Mathf.Abs(velocity.x) > 0.2f && particleOffDelayTimer >= 0)
        {
            ParticleSystem.PlaybackState state = new ParticleSystem.PlaybackState();
            footEmission.rateOverTime = 15 * Mathf.Abs(velocity.x);

        }
        else
        {
            footEmission.rateOverTime = 0;
        }
        
        if (grounded)
        {
            particleOffDelayTimer = 0.2f;
        }
        else
        {
            particleOffDelayTimer -= timeController.getSpeedAdjustedDeltaTime();
        }
        if (justLanded && grounded)
        {
            groundImpact.Play();
            justLanded = false;
        }
        if (!grounded)
        {
            justLanded = true;
        }
    }

    //Author: Marvin Winkler
    //Stops direction change during attack animation
    protected override void CharacterFacingDirection(float direction)
    {
        if (!isAttacking)
        {
            base.CharacterFacingDirection(direction);
        }
    }

    //Author: Marvin Winkler
    //Waits for the animation before the level is tilted
    private void smashFishToTilt(float playerInput)
    {
        fishTimer = 0.5f;
        if(Input.GetAxisRaw("Tilt") < 0)
        {
            playerInputBuffer = -1;
        }
        else if(Input.GetAxisRaw("Tilt") > 0)
        {
            playerInputBuffer = 1;
        }
        else
        {
            playerInputBuffer = 0;
        }
        animator.SetBool("justTilted", true);
    }

    protected override void Movement(float direction)
    {
            base.Movement(direction);
        //animDir = Mathf.Abs(direction);
        //animator.SetFloat("animationDirection", animDir);
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

    // Author: Nicole Mynarek, Michelle Limbach; Marvin Winkler fixed bugges and removed hardcoded values and replaced them with variables and added hang time
    // Method overridden, double jump is possible now
    protected override void Jump()
    {
        jumpBufferTimer = jumpBuffer;

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
            if (hangTimer >= 0 && velocity.y <= 0 && canJump)
            {
                // and cooldown lower or equal to 0
                if (cooldown <= 0)
                {
                    // jumpCountLeft will be reset
                    jumpCountLeft = jumpCount;

                    // cooldown will be set
                    cooldown = jumpCooldownTime;

                    //base.Jump();
                    velocity = new Vector2(velocity.x, jumpHeight);

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
                //base.Jump();
                velocity = new Vector2(velocity.x, jumpHeight);
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

    //Author: Michelle Limbach, Marvin Winkler
    private void CrouchDown(float direction)
    {
        //Player is not crouching yet, is grounded and the Arrow down Button is pressed
        if (!crouching && grounded && direction < 0)
        {
            //Scale the CapsuleCollider and set with offset to new position, so player does not get stuck in ground
            collider.size = new Vector2(collider.size.x, collider.size.y/2);
            collider.offset = new Vector2(collider.offset.x, collider.offset.y - collider.size.y / 2);

            //Set crouching to true, so Script knows player is already crouching
            crouching = true;

            //Decrease movement speed
            moveSpeed = moveSpeed * crouchSpeedModifier;

            //Player cannot jump while crouching
            canJump = false;
        }

    }

    // Author: Michelle Limbach, Marvin Winkler
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
                    //Rescale the CapsuleCollider size and offset
                    collider.offset = new Vector2(collider.offset.x, collider.offset.y + collider.size.y / 2);
                    collider.size = new Vector2(collider.size.x, collider.size.y*2);

                    //Set crouching to false, because the player does not crouch anymore
                    crouching = false;

                    //Increase movement speed
                    moveSpeed = moveSpeed * (1 / crouchSpeedModifier);

                    canJump = true;
                }
            }

        }

    }
    //Author: Marvin Winkler
    private void addHealth()
    {
        health++;
    }

    //Author: Marvin Winkler
    private void addTimePickup()
    {
        hasTimePickup = true;
    }

    //Author: Marvin Winkler
    private void useSloMoPickup()
    {
        if (hasTimePickup)
        {
            sloMoTimer = sloMoTime;
            hasTimePickup = false;
            onUseSloMoTime();
        }
    }

    //Author: Marvin Winkler
    public override void TakeDamage(int damage, Vector2 direction)
    {
        health -= damage;
        justTookDamage = true;
        velocity = new Vector2(direction.x * knockback, knockup);
        CharacterFacingDirection(-velocity.x);
        stunnTimer = stunnTime;
        if(health <= 0)
        {
            isDead = true;
            die();
            disableInput();
        }
    }

    //Author: Marvin Winkler
    private void die()
    {
        deadFishTimer = 1.3f;
    }

    //Author: Marvin Winkler
    private void Attack()
    {
        attackTimer = attackDelay;

        if (isFacingRight)
        {
            fishTrans.localPosition = attackOffset;
        }
        else
        {
            fishTrans.localPosition = new Vector3(-attackOffset.x, attackOffset.y, attackOffset.z);
        }
        base.Attack();

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

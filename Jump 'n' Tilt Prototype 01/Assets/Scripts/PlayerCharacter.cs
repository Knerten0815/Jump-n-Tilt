using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;
using System;
using AudioControlling;

public class PlayerCharacter : Character
{
    // Author: Nicole Mynarek, Michelle Limbach, Marvin Winkler

    // variables for jumping
    public int jumpCount;                       // Possible amount of jumps
    public int jumpCountLeft;                   // Amount of jumps that are left
    private float cooldown;
    public float jumpCooldownTime;              //Minimum time between jumps
    private bool canJump = true;                //Is Player allowed to jump

    public float hangTime;                      //by Marvin Winkler, how late can the player jump after walking of a platform in seconds
    private float hangTimer;

    public float jumpBuffer;                    //by Marvin Winkler, how early can the player hit the jump button before hitting the ground and still jump in seconds
    private float jumpBufferTimer;

    // variables for crouching
    private bool crouching = false;             //Is player crouching
    private bool underPlattform;                //Checks if there is a plattform over the head of the player
    public float plattformCheckDistance;        //Distance for Raycast is set to 15, because it is the half of the Player size
    private bool inFrontOfPlayer;
    private bool behindPlayer;
    public float crouchSpeedModifier;           //Factor by which the players speed is adjusted while crouching

    // variables for wall jump, wall sliding and detection
    private RaycastHit2D lastWallcontact;
    public int wallJumpCounter;
    public bool touchesWall;                    // for wall detection
    public float wallCheckDistance;             //public Vector2 offsetRight = new Vector2(0.5f, 0);
    public LayerMask whatIsLevel;               //public Transform test;
    public LayerMask whatIsWall;
    public bool wallSliding;
    public float wallSlidingSpeed;              // can be adjusted in inspector for finding better setting
    public float wallSlideHangTime;             // Time you have to still perform a wall jump after leaving the wall in seconds
    private float wallSlideHangTimer;
    public int facingDirection;                 // has to be set to 1 because isFacingRight is set to true. Maybe needs to be in CharacterClass?
    private RaycastHit2D hit;
    public float wallJumpSpeed;                 //by Marvin Winkler, speed given to the player when jumping of a wall
    private LevelControlls.LevelControllerNew levelController; //by Marvin Winkler, used to fix wall climbing bug while level is tilted
    public float slideJumpHeightX;              //by Marvin Winkler, Jump force in X direction
    public float slideJumpHeightY;              //by Marvin Winkler, Jump force in Y direction
    private bool slideJump = false;

    private BoxCollider2D collider;

    // variables for animation by Marvin Winkler
    private Animator animator;
    private bool jumpStart;
    private float playerInputBuffer;
    private float fishTimer;                    //used for tilt
    private bool justTookDamage;
    public float stunnTime;                     //time input gets ignored after beeing hit
    private float stunnTimer;
    private float deadFishTimer;                //used to spawn the fish object during the death animation
    private bool animatedAttack;

    // particle stuff by Marvin Winkler
    private ParticleSystem footsteps;
    private ParticleSystem.EmissionModule footEmission;
    private ParticleSystem.MainModule footstepsMain;
    private float particleOffDelayTimer;        //running particles get spawned even when leaving ground for a brief moment
    private ParticleSystem groundImpact;
    private ParticleSystem.MainModule groundImpactMain;
    private bool justLanded;
    public int slideDoubleCheckLimit = 30;
    private int slideDoubleCheck = 0;//used by groundImpact

    // attack stuff by Marvin Winkler
    private Transform fishTrans;                //is set to attackOffeset
    public Vector3 attackOffset;                //Offset from player position where he attacks
    private float attackTimer;
    public float attackDelay;                   //Minimum time between attacks in seconds
    private bool isAttacking;
    private float slideAttackCooldownTimer; 
    public float slideAttackCooldown;           //Cooldown between attacks during sliding

    //pickup stuff by Marvin Winkler
    private float sloMoTimer;
    public float sloMoTime;                     //Duration of SloMoTime power up in seconds
    private int sloMoSentTimer;                 //Timer used, so the displayed string does not get constantly updated
    private float remainingSloMoTime;

    //Audio stuff by Marvin Winkler
    public Audio damageAudio, attackAudio, tiltAudio;

    //Subscribable events by Marvin Winkler
    public delegate void useSloMoTime();
    public static event useSloMoTime onUseSloMoTime;

    public delegate void fishCausedEarthquake(float playerInput);
    public static event fishCausedEarthquake onFishCausedEarthquake;

    public delegate void fishCausedEarthquakeStart(float playerInput);
    public static event fishCausedEarthquakeStart onFishCausedEarthquakeStart;

    protected override void OnEnable()
    {
        base.OnEnable();

        //by Marvin Winkler
        //+++++
        levelController = GameObject.Find("LevelController").GetComponent<LevelControlls.LevelControllerNew>();
        fishTrans = GameObject.Find("Fish").GetComponent<Transform>();

        //Particles
        footsteps = GameObject.Find("Footsteps").GetComponent<ParticleSystem>();
        footEmission = footsteps.emission;
        footstepsMain = footsteps.main;
        groundImpact = GameObject.Find("FootLanding").GetComponent<ParticleSystem>();
        groundImpactMain = groundImpact.main;

        collider = GetComponent<BoxCollider2D>();

        //Animation
        animator = GetComponent<Animator>();
        jumpStart = true;
        jumpCountLeft = jumpCount;
        justTookDamage = false;
        deadFishTimer = -101;
        sloMoTimer = 0;
        animatedAttack = true;

        //Input
        PlayerInput.onTiltDown += smashFishToTilt;
        PlayerInput.onTiltDown += disableSliding;
        PlayerInput.onSlowMoDown += useSloMoPickup;

        //Management pickup
        ManagementSystem.healthPickUpHit += addHealth;
        ManagementSystem.timePickUpHit += addTimePickup;

        //Management update
        ManagementSystem.updateTime(remainingSloMoTime);
        ManagementSystem.updatePlayerHealth(health);
        //+++++

        // by Nicole 
        PlayerInput.onJumpButtonDown += Jump;
        PlayerInput.onHorizontalDown += Movement;
        PlayerInput.onJumpButtonUp += ShortJump;
        PlayerInput.onPlayerAttackDown += Attack;

        // by Michelle
        PlayerInput.onVerticalDown += CrouchDown;
        PlayerInput.onVerticalUp += CrouchUp;
    }
    //Author: Marvin Winkler
    //Used to fix several bugs
    private void disableSliding(float a)
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

        //Marvin Winkler
        ManagementSystem.healthPickUpHit -= addHealth;
        ManagementSystem.timePickUpHit -= addTimePickup;
        PlayerInput.onTiltDown -= smashFishToTilt;
        PlayerInput.onTiltDown -= disableSliding;
        PlayerInput.onSlowMoDown -= useSloMoPickup;
    }

    // Author: Nicole Mynarek, Marvin Winkler
    protected override void ComputeVelocity()
    {
        moveDirection = Input.GetAxis("Horizontal");
        if (!isDead)
        {
            // Player only slides when there is no input
            if (moveDirection != 0)
            {
                // character looks in the direction he is moving
                CharacterFacingDirection(moveDirection);

                if((moveDirection > 0 && slideDirection.x > 0 ) || (moveDirection < 0 && slideDirection.x < 0))
                {
                                        Slide();
                  
                }
              
            }
            else
            {
                Slide();
               
            }

            //Is sliding?
            isSliding = false;
            if (!onWall && Mathf.Abs(groundNormal.y) < 0.98 && Mathf.Abs(groundNormal.y) > 0.1)
            {
                slideJump = true;
            }
            else
            {
                slideJump = false;
            }

            if (!onWall && Mathf.Abs(groundNormal.y) < 0.98 && Mathf.Abs(groundNormal.y) > 0.1 && (moveDirection == 0 || moveDirection < 0 && slideDirection.x < 0 || moveDirection > 0 && slideDirection.x > 0))
            {
                if (slideDoubleCheck<slideDoubleCheckLimit)
                {
                    slideDoubleCheck++;
                  
                }
                else
                {
                    isSliding = true;
                            }
            }
            else
            {
                slideDoubleCheck = 0;
                      }

            velocity.x -= velocity.x * airResistance;
        }

        //hang time
        if (grounded)
        {
            hangTimer = hangTime;
        }
        else
        {
            hangTimer -= timeController.getSpeedAdjustedDeltaTime();

            //Slide after physics jump
            slideDirection.x = moveDirection;
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

        //slideAttackCooldown
        if(slideAttackCooldownTimer < 0)
        {
            slideAttackCooldownTimer = 0;
        }
        else if(slideAttackCooldownTimer == 0)
        {
            //don't do anything
        }
        else
        {
            slideAttackCooldownTimer -= timeController.getSpeedAdjustedDeltaTime();
        }

        // jump cooldown
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        //On wall while level is tilted?
        if (touchesWall && levelController.getTiltStep() != 0)
        {
            onWall = true;
        }
        else
        {
            onWall = false;
        }

        //Wall sliding
        if (!isDead)
            WallSliding();

        if (wallSliding)
            isSliding = false;
        
        //SloMoTime powerup
        if(sloMoTimer > 0)
        {
            sloMoTimer -= Time.deltaTime;
        }
        else if(sloMoTimer <= 0 && timeController.getTimeSpeed() == timeController.slowTimeSpeed)
        {
            sloMoTimer = 0;
            remainingSloMoTime = 0;
            ManagementSystem.updateTime(sloMoTimer);
            onUseSloMoTime();
        }

        //SloMoTime to management system
        if(sloMoTimer > 0)
        {
            //Updates once per second
            if(sloMoSentTimer > sloMoTimer)
            {
                sloMoSentTimer = (int)sloMoTimer;
                ManagementSystem.updateTime(sloMoTimer);
            }
        }

        //Wall slide hang timer
        if (touchesWall)
        {
            wallSlideHangTimer = wallSlideHangTime;
        }
        else if(wallSlideHangTimer > 0)
        {
            wallSlideHangTimer -= timeController.getSpeedAdjustedDeltaTime();
        }

        //Wall jump timer reduces mid air movement after wall jump
        if (grounded)
            wallJumpTimer = 0;

        if (wallJumpTimer > 0)
        {
            wallJumpTimer -= timeController.getSpeedAdjustedDeltaTime();

            if (wallJumpTimer < 0)
                wallJumpTimer = 0;
        }

        //Particle system simulation speed
        footstepsMain.simulationSpeed = timeController.getTimeSpeed();
        groundImpactMain.simulationSpeed = timeController.getTimeSpeed();


        //Just for testing:
        //+++
        //if (Input.GetKeyDown(KeyCode.Mouse0))
        //{
        //    Vector3 hitDirectionTest = gameObject.transform.localPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 hitDirectionTest2D = new Vector2(hitDirectionTest.x, hitDirectionTest.y);
        //    hitDirectionTest2D.Normalize();
        //    TakeDamage(1, hitDirectionTest2D);
        //}
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
            AudioController.Instance.playFXSound(tiltAudio);
            Debug.Log("TiltQ");
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
        //+++   SEE ABOVE! ^^
    }

    //Author: Marvin Winkler
    //Particle spawn rates get adjusted
    private void playParticleSystems()
    {
        //Running
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

        //Ground impact
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
        if (wallJumpTimer < 0)
            wallJumpTimer = 0;

        //Disables movement if running in slide direction
        if (levelController.getTiltStep() > 0 && direction < 0 && slideDirection.x > 0
            || levelController.getTiltStep() < 0 && direction > 0 && slideDirection.x < 0
            || levelController.getTiltStep() == 0 && direction > 0 && slideDirection.x < 0
            || levelController.getTiltStep() == 0 && direction < 0 && slideDirection.x > 0
            || levelController.getTiltStep() > 0 && direction > 0 && slideDirection.x < 0
            || levelController.getTiltStep() < 0 && direction < 0 && slideDirection.x > 0
            || !isSliding)
        {
            //When wallJumpTimer is 0 direction gets multiplied with 1, otherwise it gets reduced
            base.Movement(direction * ((wallJumpTime - wallJumpTimer) / (wallJumpTime)));
        }
        else
        {
            Slide();
        }
    }

    // Author: Nicole Mynarek, Marvin Winkler
    private void WallSliding()
    {
        // checking if player touches wall (for wallSliding, wallJump), touchesWall is a bool
        touchesWall = (Physics2D.Raycast((Vector2)transform.position, transform.right, wallCheckDistance, whatIsWall) || Physics2D.Raycast((Vector2)transform.position, -transform.right, wallCheckDistance, whatIsWall));

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
        if (onWall) //only when on wall and level is tilted
            return;

        if (!touchesWall && !grounded && jumpCountLeft <= 0 && wallSlideHangTimer <= 0)
        {
            jumpBufferTimer = jumpBuffer;
            return;
        }

        // if touchesWall is true, player can do a wallJump
        if (wallSliding)
        {
            hit = Physics2D.Raycast((Vector2)transform.position, transform.right, wallCheckDistance, whatIsWall);
            lastWallcontact = Physics2D.Raycast((Vector2)transform.position, -transform.right, wallCheckDistance, whatIsWall);
            if (hit.distance < lastWallcontact.distance)
            {
                hit = lastWallcontact;
            }
            WallJump(false);
            if((hit.point - new Vector2(transform.position.x, transform.position.y)).x < 0)
            {
                CharacterFacingDirection(1);
            }
            else
            {
                CharacterFacingDirection(-1);
            }
        }
        else if(wallSlideHangTimer > 0)
        {
            hit = Physics2D.Raycast((Vector2)transform.position, transform.right, wallCheckDistance * 3, whatIsWall);
            lastWallcontact = Physics2D.Raycast((Vector2)transform.position, -transform.right, wallCheckDistance * 3, whatIsWall);
            if (hit.distance < lastWallcontact.distance)
            {
                hit = lastWallcontact;
            }
            WallJump(true);
            if ((hit.point - new Vector2(transform.position.x, transform.position.y)).x < 0)
            {
                CharacterFacingDirection(1);
            }
            else
            {
                CharacterFacingDirection(-1);
            }
        }
        else
        {
            // if player is on the ground
            if (hangTimer >= 0 && velocity.y <= 0 && canJump)
            {
                // and cooldown lower or equal to 0
                if (cooldown <= 0)
                {
                    // reset of wallJumpCounter
                    wallJumpCounter = 5;
                    lastWallcontact = new RaycastHit2D();

                    // cooldown will be set
                    cooldown = jumpCooldownTime;

                    if (isSliding)
                    {
                        velocity = new Vector2(velocity.x + slideDirection.x * slideJumpHeightX, slideJumpHeightY);
                        Debug.Log("Slide values + " + velocity.normalized.x + " "+ velocity.normalized.y);
                    }
                    else if (slideJump){
                        velocity = new Vector2(-1*moveDirection/2, slideJumpHeightY/10);
                        Debug.Log("Slide Jump values + " + velocity.normalized.x + " " + velocity.normalized.y);

                    }
                    else
                    {
                        velocity = new Vector2(moveDirection * maxAirMovementSpeed, jumpHeight);
                        //Debug.Log("Jump Values + " + velocity.normalized.x + " " + velocity.normalized.y);

                    }

                    // jumpCountLeft will be reset
                    jumpCountLeft = jumpCount;

                    jumpCountLeft--;
                }
            }
            // if jumpCountLeft is lower than or equal to 0, player can not jump anymore
            else if (jumpCountLeft <= 0)
            {
                jumpable = false;
            }
            else
            //double jump
            {
                jumpable = true;

                // cooldown is set
                cooldown = jumpCooldownTime;

                velocity = new Vector2(velocity.x + moveDirection * maxAirMovementSpeed, jumpHeight);

                jumpCountLeft--;

            }
        }
    }

    //Debugged and movified by Marvin Winkler
    private void WallJump(bool hangTimeJump)
    {
        //resets wallJumpCounter, so there is no limit for wall jumps on the same wall
        wallJumpCounter = 500;
        //Resets double jumps
        jumpCountLeft = jumpCount;

        // player can jump if canJump is true 
        // or if the wallJumpCounter is higher than 0
        if (canJump && (wallJumpCounter > 0 && !hangTimeJump)) //Player springt ab
        { 
            wallSliding = false;
            jumpCountLeft--;
            velocity = new Vector2(hit.normal.x * wallJumpSpeed, jumpHeight);
            //CharacterFacingDirection(hit.normal.x);
            jumpable = false;
            //lastWallcontact = hit;
            wallJumpCounter--;
            wallJumpTimer = wallJumpTime;
        }
        //Hang time wall jump
        else if (hangTimeJump)
        {
            wallSliding = false;
            jumpCountLeft--;
            velocity = new Vector2(hit.normal.x * wallJumpSpeed, jumpHeight);
            if (Input.GetAxis("Horizontal") < 0)
            {
                //CharacterFacingDirection(-hit.normal.x);
                //velocity = new Vector2(-Mathf.Abs(hit.normal.x) * wallJumpSpeed, jumpHeight);
            }
            else
            {
                //CharacterFacingDirection(hit.normal.x);
                //velocity = new Vector2(Mathf.Abs(hit.normal.x) * wallJumpSpeed, jumpHeight);
            }
            jumpable = false;
            //lastWallcontact = hit;
            wallJumpCounter--;
            wallJumpTimer = wallJumpTime;
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

            //The next two are nessesary so the player does not glitch thru the platform when just coming out from beneath it
            underPlattform = underPlattform || Physics2D.Raycast((Vector2)transform.position, new Vector2(1, 1), plattformCheckDistance, whatIsLevel);
            underPlattform = underPlattform || Physics2D.Raycast((Vector2)transform.position, new Vector2(-1, 1), plattformCheckDistance, whatIsLevel);

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
        ManagementSystem.updatePlayerHealth(health);
    }

    //Author: Marvin Winkler
    private void addTimePickup()
    {
        remainingSloMoTime += sloMoTime;
        ManagementSystem.updateTime(remainingSloMoTime);
    }

    //Author: Marvin Winkler
    //Slomo time gets toggled
    private void useSloMoPickup()
    {
        if(timeController.getTimeSpeed() == timeController.slowTimeSpeed) //Slow
        {
            remainingSloMoTime = sloMoTimer;
            sloMoSentTimer = (int)sloMoTimer;
            sloMoTimer = 0;
            onUseSloMoTime();
            return;
        }
        if (remainingSloMoTime > 0) //Fast and time left
        {
            sloMoTimer = remainingSloMoTime;
            sloMoSentTimer = (int)sloMoTimer;
            onUseSloMoTime();
        }
    }

    //Author: Marvin Winkler
    //Takes damage and gets knocked back, overrides the charackter takeDamage() funktion
    public override void TakeDamage(int damage, Vector2 direction)
    {
        health -= damage;
        ManagementSystem.updatePlayerHealth(health);
        justTookDamage = true;
        velocity = new Vector2(direction.x * knockback, knockup);
        CharacterFacingDirection(-velocity.x);
        stunnTimer = stunnTime;
        Instantiate(bloodSpray, transform.position, Quaternion.identity);
        AudioController.Instance.playFXSound(damageAudio);
        if (health <= 0)
        {
            isDead = true;
            die();
            disableInput();
        }
    }

    //Author: Marvin Winkler
    //Only used for death animation
    private void die()
    {
        deadFishTimer = 1.3f;
    }

    //Author: Marvin Winkler
    //The fish position gets updated based on the facing direction, then Characters base attack gets called
    protected override void Attack()
    {
        //During slide the attack is not animated
        if (animatedAttack)
        {
            attackTimer = attackDelay;
            AudioController.Instance.playFXSound(attackAudio);
        }

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

    //Author: Marvin Winkler
    protected override void Slide()
    {
        //No slide when on wall while tilted
        if (onWall)
        {
            return;
        }

        //auto attack during slide
        if (velocity.magnitude > slideBackwardsMaxSpeed && slideAttackCooldownTimer <= 0 && isSliding)
        {
            slideAttackCooldownTimer = slideAttackCooldown;
            animatedAttack = false;
            Attack();
            animatedAttack = true;
        }

        //Slides down, runs up
        if(slideDirection.x < 0 && moveDirection < 0 || slideDirection.x > 0 && moveDirection > 0 || moveDirection == 0)
        base.Slide();
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

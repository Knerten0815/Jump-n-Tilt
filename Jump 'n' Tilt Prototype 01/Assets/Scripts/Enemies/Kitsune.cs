using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Katja Tuemmers
//Based on Kappa written by Nicole Mynarek, it adds a cooldown to prevent the Kitsune from damaging 
//the player immediately after being hit and also passes on information to the EndbossContainer
//It removes sliding and lets the kitsune be damaged from any direction
public class Kitsune : GroundEnemy
{
    //EndBoss container is a object that passes information back and forth between EndBoss and Kitsune
    [SerializeField]
    private EndBossContainer endBoss;

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
    private Coroutine coolDamageRoutine;

    private bool hasDamaged;
    [SerializeField]
    private GameObject sparkle;

    protected BoxCollider2D bc2d;

    public Audio kitsuneAttack;
    public Audio kitsuneHit, kitsuneDeath;

    public GameObject deathSparkle;

    void Start()
    {
        Instantiate(sparkle, transform.position, Quaternion.identity);

        base.Start();
        lastYPos = transform.position.y;
        attackRadius = 1.2f;
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        attackPos.position = new Vector3(bc2d.bounds.center.x, bc2d.bounds.center.y, 0f);
    }
    /*
    * ComputeVelocity is basically the same as in the Kappa but removes Sliding
    */
    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        if (isIdle && Mathf.Abs(playerDirection().x) < distanceToPlayer)
        {
            currentIdleTime += Time.deltaTime;

            if (currentIdleTime >= idleTime)
            {
                currentIdleTime = 0;
                Jump();
            }
        }

        if (grounded == true && isJumping == false)
        {
            isIdle = true;
            Debug.Log("isidle " + isIdle);
            isJumping = false;
            isFalling = false;

            attackPos.position = new Vector3(bc2d.bounds.center.x, bc2d.bounds.center.y, 0f);

            anim.SetBool("isJumping", false);
            anim.SetBool("isJumpingDown", false);
            anim.SetBool("isIdle", true);
            //jumpStart = true;
        }
        else if (transform.position.y > lastYPos && grounded == false && isIdle == false)
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
            anim.SetBool("isIdle", false);
            //anim.SetBool("jumpStart", jumpStart);
            //jumpStart = false;
        }
        else if (transform.position.y < lastYPos && grounded == false && isIdle == false)
        {
            isJumping = false;
            isFalling = true;

            anim.SetBool("isJumpingDown", true);
        }

        lastYPos = transform.position.y;

        if (GameObject.Find("Player").GetComponent<PlayerCharacter>().health > 0)
        {
            airMovement();
        }
    }
    //Same as in Kappa by Nicole Mynarek
    protected override void Jump()
    {
        isSliding = false;
        isIdle = false;
        isJumping = true;

        if (playerDirection().x < 0f)
        {
            direction = 1f;
            CharacterFacingDirection(direction);
        }
        else
        {
            direction = -1f;
            CharacterFacingDirection(direction);
        }

        if (GameObject.Find("Player").GetComponent<PlayerCharacter>().health == 0)
        {
            velocity = new Vector2(0f, 0f);
        }
        else
        {
            velocity = new Vector2(playerDirection().normalized.x * jumpDistance, jumpHeight);
        }
    }
    //Same as in Kappa by Nicole Mynarek
    protected void airMovement()
    {
        if (!grounded && direction != 0 && velocity.magnitude < maxAirMovementSpeed)
        {
            velocity += (direction * moveWhileJumping) * Vector2.right * (1 - wallJumpTime) * (1 / ((0.1f + Mathf.Abs(velocity.x) * 0.5f))); //velocity = new Vector2((velocity.x + (moveWhileJumping * moveDirection)) * Mathf.Pow(airResistance, velocity.magnitude) * (1 - wallJumpTime), velocity.y);
            isSliding = false;
        }
    }
    //TakeDamage was modifed to pass on health to EndBossContainer
    //Kitsune can be damaged from any direction
    //It sets the new bool hasDamage to true and blocks any further damage or attack by Kitsune until its set to true again
    //TakeDamage starts a Coroutine that waits a certain amount of time until it sets hasDamage to true and making new damage or attacks possible
    public override void TakeDamage(int damage, Vector2 direction)
    {
        if (!hasDamaged)
        {
            hasDamaged = true;

            if (health <= 1)
            {
                Instantiate(deathSparkle, transform.position, Quaternion.identity);
                AudioController.Instance.playFXSound(kitsuneDeath);
            }
            else
            {
                AudioController.Instance.playFXSound(kitsuneHit);
            }
         
            base.TakeDamage(damage, direction);
            endBoss.passOnHealth(health);
            coolDamageRoutine = StartCoroutine(damageCooldown(1.0f));
       
        }
    }
    //adds hasDamage condition
    public override void groundEnemyAttack(Collider2D enemy, Vector2 dmgDirection2D)
    {
        if (!hasAttacked && getPlayerScript().health > 0 && !hasDamaged)
        {
            AudioController.Instance.playFXSound(kitsuneAttack);
            base.groundEnemyAttack(enemy, dmgDirection2D);
        }
    }

    //The damageCooldown routine that set hasDamaged back to true after awhile
    IEnumerator damageCooldown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        hasDamaged = false;
        StopCoroutine(coolDamageRoutine);
    }

}

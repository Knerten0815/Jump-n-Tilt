using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitsune : GroundEnemy
{
    // Start is called before the first frame update
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


    protected BoxCollider2D bc2d;

    public Audio kappaJump;
    public Audio kappaHit, kappaBlock;

    public GameObject dust;

    void Start()
    {
        base.Start();
        health = 8;
        lastYPos = transform.position.y;
        attackRadius = 1.2f;
        anim = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        attackPos.position = new Vector3(bc2d.bounds.center.x, bc2d.bounds.center.y, 0f);
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();


        if (isIdle && Mathf.Abs(playerDirection().x) < distanceToPlayer && !isSliding)
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
            isJumping = false;
            isFalling = false;

            attackPos.position = new Vector3(bc2d.bounds.center.x, bc2d.bounds.center.y, 0f);

            anim.SetBool("isJumping", false);
            anim.SetBool("isIdle", true);
            jumpStart = true;
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
            anim.SetBool("jumpStart", jumpStart);
            jumpStart = false;
        }
        else if (transform.position.y < lastYPos && grounded == false && isIdle == false)
        {
            isJumping = false;
            isFalling = true;
        }

        lastYPos = transform.position.y;

        if (GameObject.Find("Player").GetComponent<PlayerCharacter>().health > 0)
        {
            airMovement();
        }
    }

    protected override void Jump()
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
        AudioController.Instance.playFXSound(kappaJump);

        if (GameObject.Find("Player").GetComponent<PlayerCharacter>().health == 0)
        {
            velocity = new Vector2(0f, jumpHeight);
        }
        else
        {
            velocity = new Vector2(playerDirection().normalized.x * jumpDistance, jumpHeight);
        }
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
        if (!hasDamaged)
        {
            hasDamaged = true;
            
            Debug.Log("player direction: " + playerDirection().x);
            AudioController.Instance.playFXSound(kappaHit);
            base.TakeDamage(damage, direction);
            endBoss.damageTaken();
            coolDamageRoutine = StartCoroutine(damageCooldown(1));
        }
    }
    public override void groundEnemyAttack(Collider2D enemy, Vector2 dmgDirection2D)
    {
        if (!hasAttacked && !isSliding && getPlayerScript().health > 0 && !hasDamaged)
        {
            base.groundEnemyAttack(enemy, dmgDirection2D);
        }
    }
    IEnumerator damageCooldown(float coolDownTime)
    {
        //Debug.Log(coolDownTime + " seconds Cooldown!");
        yield return new WaitForSeconds(coolDownTime);
        //Debug.Log("Cooldown end!");
        hasDamaged = false;
        StopCoroutine(coolDamageRoutine);
    }
}

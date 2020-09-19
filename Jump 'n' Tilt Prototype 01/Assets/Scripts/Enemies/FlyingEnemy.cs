using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Nicole Mynarek
public class FlyingEnemy : Enemy
{
    private enum State
    {
        Walking,
        ChaseTarget,
        Win,
        Knockback,
    }

    private Vector2 startPos;                   //start position of FlyingEnemy
    public float gravityCounter;                //time for changing the y-variable from velocity
    private float gravitySwitchCounter;         //timer, starting at value of gravityCounter
    public float velY;                          //y value of velocity
    public float targetRange;                   //distance from which FlyingEnemy chases the player
    public float attackRange;                   //distance from which FlyingEnemy attacks the player

    public Vector2 roamPos;                     //specifies the range over which FlyingEnemy can move
    private State state; 
    [SerializeField] float attackCooldownTime;
    protected bool movesRight = true;           //true if FlyingEnemy moves right
    protected bool hasAttacked = false;         //true if FlyingEnemy has attacked
    protected bool isAttacking;                 //true if FlyingEnemy attacks
    protected bool isWalking;                   //true if FlyingEnemy "walks" during default state
    protected bool isChasing;                   //true if FlyingEnemy chases player
    private Coroutine coolroutine;

    private void Awake()
    {
        state = State.Walking;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        whatIsEnemy = LayerMask.GetMask("Player");
    }

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;          //current position is set as start position
        player = GameObject.Find("Player");
        grounded = true;
        gravitySwitchCounter = gravityCounter;
    }
    protected override void ComputeVelocity()
    {
        //if player dies, state changes to Win
        if(player.GetComponent<PlayerCharacter>().health == 0)
        {
            state = State.Win;
        }

        base.ComputeVelocity();

        switch (state)
        {
            //state, where FlyingEnemy chases player
            case State.ChaseTarget:

                isChasing = true;
                isAttacking = false;

                //FlyingEnemy comes from right side
                if (transform.position.x > player.transform.position.x)
                {
                    movesRight = false;
                    moveDirection = -1;
                    gravityModifier = 0f;

                    //by Katja Tuemmers
                    //calculating direction to player and adding the vector to velocity to increase FlyingEnemy's speed
                    Vector2 toPlayer = new Vector2((player.transform.position.x - transform.position.x), player.transform.position.y - transform.position.y);
                    velocity += toPlayer.normalized;
                    
                }
                //FlyingEnemy comes from left side
                else
                {
                    movesRight = true;
                    moveDirection = 1;
                    gravityModifier = 0f;

                    //by Katja Tuemmers
                    //calculating direction to player and adding the vector to velocity to increase FlyingEnemy's speed
                    Vector2 toPlayer = new Vector2((player.transform.position.x - transform.position.x), player.transform.position.y - transform.position.y);
                    velocity += toPlayer.normalized;
                    
                }

                //if player falls below a distance FlyingEnemy attacks
                if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                {
                    isAttacking = true;
                    Attack();
                }

                //if player keeps enough distance FlyingEnemy returns to his default state
                if (Vector3.Distance(transform.position, player.transform.position) > targetRange)
                {
                    state = State.Walking;
                }
                break;

            //state, if FlyingEnemy killed player
            case State.Win:

                Movement(0);            //no horizontal movement, only vertical movement
                isAttacking = false;
                isChasing = false;
                isWalking = false;

                break;

            //state, to knock FlyingEnemy back after he attacked player
            case State.Knockback:

                velocity = new Vector2(moveDirection * -knockback, knockup);

                // if player keeps enough distance, FlyingEnemy returns to his default state
                if (movesRight)
                {

                    if(playerDirection().x > 0f && Mathf.Abs(playerDirection().x) > targetRange)
                    {
                        state = State.Walking;
                    }
                }
                else
                {

                    if (playerDirection().x < 0f && Mathf.Abs(playerDirection().x) > targetRange)
                    {
                        state = State.Walking;
                    }
                }

                break;
            
            default:

                isAttacking = false;
                isChasing = false;
                isWalking = true;

                //FlyingEnemy moves in a specific area
                if (movesRight)
                {
                    Movement(1);
                    Vector2 goal = startPos + roamPos;      //border of FlyingEnemy's moving area
                    if (transform.position.x >= goal.x)     //turns around at the border
                    {
                        movesRight = false;
                        startPos = transform.position;      //position at border is new start position
                    }
                }
                else
                {
                    Movement(-1);
                    Vector2 goal = startPos - roamPos;      //border of FlyingEnemy's moving area
                    if (transform.position.x <= goal.x)     //turns around at the border
                    {
                        movesRight = true;
                        startPos = transform.position;      //position at border is new start position
                    }
                }

                FindTarget();
                break;
        }

    }

    //method for basic flying movement
    //FlyingEnemy moves vertical while moving horizontal
    protected override void Movement(float direction)
    {
        moveDirection = direction;

        //timer runs down
        if (gravitySwitchCounter >= 0)
        {
            gravitySwitchCounter -= Time.deltaTime;
        }
        else
        {
            //if timer reaches 0, it will be reset
            gravitySwitchCounter = gravityCounter;
        }

        //if the timer is lower than the half of the total time, FlyingEnemy moves up
        if (gravitySwitchCounter < (gravityCounter / 2f))
        {
            velocity = new Vector2(moveDirection * moveSpeed, velY);
        }
        //if the timer is greater than the half of the total time, FlyingEnemy moves down
        else
        {
            velocity = new Vector2(moveDirection * moveSpeed, -velY);
        }
    }

    //method for finding player
    private void FindTarget()
    {
        //if player falls below a distance FlyingEnemy changes state to ChaseTarget
        if (Vector3.Distance(transform.position, player.transform.position) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }

    //method for attacking player
    //damages all enemies of the character within the attack radius of the attack position
    protected override void Attack()
    {
        if (hasAttacked == false)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);          //array for collider that are detected in the attackRadius

            for (int i = 0; i < enemies.Length; i++)
            {
                Vector3 dmgDirection = gameObject.transform.localPosition - enemies[i].GetComponent<Transform>().localPosition;     //calculating direction the damage comes from
                Vector2 dmgDirection2D = new Vector2(dmgDirection.x, dmgDirection.y);
                dmgDirection2D.Normalize();
                enemies[i].GetComponent<PlayerCharacter>().TakeDamage(1, -dmgDirection2D);      //TakeDamage function of player is called
                
                hasAttacked = true;
                coolroutine = StartCoroutine(attackCooldown(attackCooldownTime));       //cooldown after one attack
                state = State.Knockback;                                                //state change, FlyingEnemy gets knocked back
            }
        }
    }

    //Author: Katja Tuemmers
    IEnumerator attackCooldown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        hasAttacked = false;
        StopCoroutine(coolroutine);
    }

    //sets FlyerCollider object inactive to not affect other characters in the scene
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.SetActive(false);
    }
}

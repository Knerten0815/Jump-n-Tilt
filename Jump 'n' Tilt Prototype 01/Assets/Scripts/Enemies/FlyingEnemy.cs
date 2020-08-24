using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Character
{
    private enum State
    {
        Walking,
        ChaseTarget,
        Win,
        Wait,
    }

    private Vector2 startPos;
    public float gravityCounter;
    private float gravitySwitchCounter;
    public float velY;
    public float waitTime;
    private float waitTimeCounter;

    public Vector2 roamPos;
    private State state;
    private GameObject player;

    public bool movesRight = true;
    public bool hasAttacked = false;
    protected bool isAttacking;
    protected bool isWalking;
    protected bool isChasing;

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
        startPos = transform.position;
        player = GameObject.Find("Player");
        grounded = true;
        gravitySwitchCounter = gravityCounter;
    }
    protected override void ComputeVelocity()
    {

        if(player.GetComponent<PlayerCharacter>().health == 0)
        {
            state = State.Win;
        }

        base.ComputeVelocity();

        if (isDead)
        {
            gameObject.SetActive(false);
        }

        switch (state)
        {
            case State.ChaseTarget:

                isChasing = true;
                isAttacking = false;

                if (transform.position.x > player.transform.position.x)
                {
                    movesRight = false;
                    moveDirection = -1;
                    gravityModifier = 0f;

                    Vector2 toPlayer = new Vector2((player.transform.position.x - transform.position.x), player.transform.position.y - transform.position.y);
                    /*if (!(velocity.x < 0 && toPlayer.x < 0) || !(velocity.y < 0 && toPlayer.y < 0))
                    {
                        Debug.Log("nicht schneller");
                        velocity = toPlayer.normalized;
                    }
                    else
                    {*/
                        Debug.Log("Schneller!");
                        velocity += toPlayer.normalized;
                    //}
                }
                else
                {
                    movesRight = true;
                    moveDirection = 1;
                    gravityModifier = 0f;

                    Vector2 toPlayer = new Vector2((player.transform.position.x - transform.position.x), player.transform.position.y - transform.position.y);
                    /*if (!(velocity.x > 0 && toPlayer.x > 0) || !(velocity.y < 0 && toPlayer.y < 0))
                    {
                        Debug.Log("nicht schneller");
                        velocity = toPlayer.normalized;
                    }
                    else
                    {*/
                        Debug.Log("Schneller!");
                        velocity += toPlayer.normalized;
                    //}

                    Debug.Log("velocity onryo: " + velocity + " velocity toplayer: " + toPlayer);
                }

                float attackRange = 1f;
                if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                {
                    Attack();
                    isAttacking = true;
                }

                float targetRange = 5f;
                if (Vector3.Distance(transform.position, player.transform.position) > targetRange)
                {
                    state = State.Walking;
                }
                break;

            case State.Win:

                Debug.Log("Player ist tot.");
                Movement(0);
                isAttacking = false;
                isChasing = false;
                isWalking = false;

                break;

            case State.Wait:
                Debug.Log("FlyingEnemy wartet.");
                Movement(0);

                Vector2 awayFromPlayer = new Vector2(transform.position.x + 1f, transform.position.y + 1f);
                velocity = awayFromPlayer;

                isAttacking = false;
                isChasing = false;
                isWalking = false;

                if (Vector3.Distance(transform.position, player.transform.position) < 5f)
                {
                    state = State.Walking;
                }

                break;


            default:

                Debug.Log("State walking");

                isAttacking = false;
                isChasing = false;
                isWalking = true;

                if (movesRight)
                {
                    Movement(1);
                    Vector2 goal = startPos + roamPos;
                    if (transform.position.x >= goal.x)
                    {
                        movesRight = false;
                        startPos = transform.position;
                    }
                }
                else
                {
                    Movement(-1);
                    Vector2 goal = startPos - roamPos;
                    if (transform.position.x <= goal.x)
                    {
                        movesRight = true;
                        startPos = transform.position;
                    }
                }

                FindTarget();
                break;
        }

    }

    protected override void Movement(float direction)
    {
        moveDirection = direction;

        //Debug.Log(gravitySwitchCounter);

        if (gravitySwitchCounter >= 0)
        {
            gravitySwitchCounter -= Time.deltaTime;
        }
        else
        {
            gravitySwitchCounter = gravityCounter;
        }

        if (gravitySwitchCounter < (gravityCounter / 2f))
        {
            velocity = new Vector2(moveDirection * moveSpeed, velY);
        }
        else
        {
            velocity = new Vector2(moveDirection * moveSpeed, -velY);
        }
    }

    private void FindTarget()
    {
        float targetRange = 5f;
        if (Vector3.Distance(transform.position, player.transform.position) < targetRange)
        {
            state = State.ChaseTarget;
        }
    }

    protected override void Attack()
    {
        //isAttacking = true;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 dmgDirection = gameObject.transform.localPosition - enemies[i].GetComponent<Transform>().localPosition;
            Vector2 dmgDirection2D = new Vector2(dmgDirection.x, dmgDirection.y);
            dmgDirection2D.Normalize();

            if (hasAttacked == false)
            {
                enemies[i].GetComponent<PlayerCharacter>().TakeDamage(1, dmgDirection2D);
                Debug.Log("Tengus enemy: " + enemies[i]);

                Debug.Log("velocity onryo: " + velocity);

                //hasAttacked = true;

                //state = State.Wait;

                if (movesRight)
                {
                    Vector2 awayFromPlayer = new Vector2(transform.position.x - 6f - transform.position.x, transform.position.y);
                    //velocity = new Vector2(moveDirection * 10, 0);
                    transform.position = new Vector3(transform.position.x - 1f, transform.position.y, 0);
                    Debug.Log("away: " + awayFromPlayer);
                }
                else
                {
                    Vector2 awayFromPlayer = new Vector2((transform.position.x + 5f) - transform.position.x, transform.position.y);
                    //velocity += awayFromPlayer.normalized;
                    transform.position = new Vector3(transform.position.x + 1f, transform.position.y, 0);
                    Debug.Log("away: " + awayFromPlayer);
                }
            }

            if(enemies.Length == 0)
            {
                hasAttacked = false;
            }
            
        }
        
        //base.Attack();
        Debug.Log("ATTACK!!!!!!!!!!!!!!!!!");
        Debug.Log("enemies: " + enemies);
        Debug.Log("enemies length: " + enemies.Length);
        Debug.Log("enemy: " + enemies[0]);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("AAAAAAH");
        collision.gameObject.SetActive(false);
    }
}

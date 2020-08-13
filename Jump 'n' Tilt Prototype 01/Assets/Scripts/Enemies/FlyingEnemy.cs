using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Character
{
    private enum State
    {
        Walking,
        ChaseTarget,
    }

    private Vector2 startPos;
    public float gravityCounter;
    private float gravitySwitchCounter;
    public float velY;

    public Vector2 roamPos;
    private State state;
    private GameObject player;

    public bool movesRight = true;
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
        base.ComputeVelocity();

        if (isDead)
        {
            gameObject.SetActive(false);
        }

        switch (state)
        {
            case State.ChaseTarget:

                isChasing = true;

                if (transform.position.x > player.transform.position.x)
                {
                    movesRight = false;
                    moveDirection = -1;
                    gravityModifier = 0f;

                    Vector2 toPlayer = new Vector2((player.transform.position.x - transform.position.x), player.transform.position.y - transform.position.y);
                    if (!(velocity.x < 0 && toPlayer.x < 0) || !(velocity.y < 0 && toPlayer.y < 0))
                    {
                        Debug.Log("nicht schneller");
                        velocity = toPlayer.normalized;
                    }
                    else
                    {
                        Debug.Log("Schneller!");
                        velocity += toPlayer.normalized;
                    }
                }
                else
                {
                    movesRight = true;
                    moveDirection = 1;
                    gravityModifier = 0f;

                    Vector2 toPlayer = new Vector2((player.transform.position.x - transform.position.x), player.transform.position.y - transform.position.y);
                    if (!(velocity.x > 0 && toPlayer.x > 0) || !(velocity.y < 0 && toPlayer.y < 0))
                    {
                        Debug.Log("nicht schneller");
                        velocity = toPlayer.normalized;
                    }
                    else
                    {
                        Debug.Log("Schneller!");
                        velocity += toPlayer.normalized;
                    }

                    Debug.Log("velocity onryo: " + velocity + " velocity toplayer: " + toPlayer);
                }

                float attackRange = 1f;
                if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                {
                    Attack();
                }

                float targetRange = 5f;
                if (Vector3.Distance(transform.position, player.transform.position) > targetRange)
                {
                    state = State.Walking;
                }
                break;

            default:

                Debug.Log("State walking");

                isAttacking = false;
                isChasing = false;

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

        Debug.Log(gravitySwitchCounter);

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
        isAttacking = true;
        base.Attack();
        Debug.Log("ATTACK!!!!!!!!!!!!!!!!!");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("AAAAAAH");
        collision.gameObject.SetActive(false);
    }
}

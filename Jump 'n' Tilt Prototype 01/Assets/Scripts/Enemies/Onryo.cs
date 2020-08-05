using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Nicole Mynarek
public class Onryo : Character
{
    private enum State
    {
        Walking,
        ChaseTarget,
    }

    private Vector2 startPos;
    //private Vector2 range;
    //private Vector2 goal;
    public float gravitySwitchCounter;

    public Vector2 roamPos;
    private State state;
    private GameObject player;

    public bool movesRight = true;

    private Animator anim;

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
        //Debug.Log("Start: " + startPos);
        player = GameObject.Find("Player");
        //range = new Vector2(Random.Range(-7f, 7f), Random.Range(-7f, 7f));
        //goal = startPos + range;
        anim = GetComponent<Animator>();
    }
    protected override void ComputeVelocity()
    {
        Debug.Log("grounded: " + grounded);
        base.ComputeVelocity();

        switch (state)
        {
            default:
            case State.Walking:

                anim.SetBool("isAttacking", false);

                if (movesRight)
                {
                    Movement(1);
                    Vector2 goal = startPos + roamPos;
                    //Debug.Log("Goal positiv: " + goal);
                    //Debug.Log("aktuelle Position: " + transform.position);
                    if (transform.position.x >= goal.x /*&& transform.position.y == goal.y*/)
                    {
                        movesRight = false;
                        startPos = transform.position;
                    }
                    Debug.Log("Onryo geht nach rechts");
                }
                else
                {
                    Movement(-1);
                    Vector2 goal = startPos - roamPos;
                    //Debug.Log("Goal negativ: " + goal);
                    if (transform.position.x <= goal.x /*&& transform.position.y == goal.y*/)
                    {
                        movesRight = true;
                        startPos = transform.position;
                    }
                    //Debug.Log("Onryo geht nach links");
                }

                FindTarget();
                break;

            case State.ChaseTarget:
                //Debug.Log("Player in der Nähe");

                anim.SetBool("isAttacking", true);

                if (transform.position.x > player.transform.position.x)
                {
                    movesRight = false;
                    moveDirection = -1;
                    gravityModifier = 0f;
                    Vector2 toPlayer = new Vector2((player.transform.position.x - transform.position.x), player.transform.position.y - transform.position.y);
                    velocity = toPlayer.normalized;


                }
                else
                {
                    movesRight = true;
                    moveDirection = 1;
                    gravityModifier = 0f;
                    Vector2 toPlayer = new Vector2((player.transform.position.x - transform.position.x), player.transform.position.y - transform.position.y);
                    velocity = toPlayer.normalized;
                }

                /*float attackRange = 1f;
                if (Vector3.Distance(transform.position, player.transform.position) < attackRange)
                {
                    Attack();
                }*/

                float targetRange = 5f;
                if (Vector3.Distance(transform.position, player.transform.position) > targetRange)
                {
                    state = State.Walking;
                }
                break;
        }

    }

    protected override void Movement(float direction)
    {
        Debug.Log("Movement test");
        moveDirection = direction;

        if (gravitySwitchCounter >= 0)
        {
            gravitySwitchCounter -= Time.deltaTime;
        }
        else
        {
            gravitySwitchCounter = 4f;
        }

        if (gravitySwitchCounter < 2f)
        {
            velocity = new Vector2(moveDirection * moveSpeed, 0.5f);
        }
        else
        {
            velocity = new Vector2(moveDirection * moveSpeed, -0.5f);
        }

        //velocity = new Vector2(moveDirection * moveSpeed, -4f);
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
        //base.Attack();
        Debug.Log("Attacke!!!");

        /*Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Character>().TakeDamage(1);
        }*/
    }
    public override void TakeDamage(int damage)
    {
        health -= damage;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public float moveCounter;

    protected override void OnEnable()
    {
        base.OnEnable();
        whatIsEnemy = LayerMask.GetMask("Player");
    }

    protected override void Start()
    {
        base.Start();
        moveCounter = 6f;
    }
    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        if (moveCounter >= 0)
        {
            moveCounter -= Time.deltaTime;
        }
        else
        {
            moveCounter = 6f;
        }

        if (moveCounter < 3)
        {
            Movement(-1);
        }
        else
        {
            Movement(1);
        }

    }
    public override void TakeDamage(int damage, Vector2 direction)
    {
        base.TakeDamage(damage, direction);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onryo : Character
{
    public bool movesRight = true;
    protected override void OnEnable()
    {
        base.OnEnable();
        whatIsEnemy = LayerMask.GetMask("Player");
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        if (movesRight)
        {
            Movement(1);
        }
        else
        {
            Movement(-1);
        }
    }
    protected override void TakeDamage(int damage)
    {
        health -= damage;
    }
}

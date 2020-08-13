using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tengu : FlyingEnemy
{

    private Animator anim;

    protected override void OnEnable()
    {
        base.OnEnable();
        anim = GetComponent<Animator>();
    }
    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        if (isChasing)
        {
            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }
    }
}

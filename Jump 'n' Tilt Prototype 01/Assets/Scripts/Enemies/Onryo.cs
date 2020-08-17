using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Nicole Mynarek
public class Onryo : FlyingEnemy
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

        if (isAttacking)
        {
            anim.SetBool("hitting", true);
        }
        else
        {
            anim.SetBool("hitting", false);
        }
    }
}


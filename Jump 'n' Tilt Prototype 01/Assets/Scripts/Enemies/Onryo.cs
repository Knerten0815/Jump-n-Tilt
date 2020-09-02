using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioControlling;

//Author: Nicole Mynarek
// testkommentar
public class Onryo : FlyingEnemy
{
    private Animator anim;
    public Audio onryoIdle, onryoHit, onryoDeath;

    protected override void OnEnable()
    {
        base.OnEnable();
        anim = GetComponent<Animator>();
    }
    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        anim.speed = timeController.getTimeSpeed();

        if (isAttacking)
        {
            anim.SetBool("hitting", true);
            AudioController.Instance.playFXSound(onryoIdle);
        }
        else
        {
            anim.SetBool("hitting", false);
        }
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        AudioController.Instance.playFXSound(onryoDeath);
        base.TakeDamage(damage, direction);
    }
}


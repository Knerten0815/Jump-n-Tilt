using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioControlling;


public class Tengu : FlyingEnemy
{

    private Animator anim;
    private bool hitWall;
    private bool hitPlatform;
    private LayerMask wall;
    private LayerMask platform;

    public Audio tenguHit, tenguAttack;

    protected override void OnEnable()
    {
        base.OnEnable();
        anim = GetComponent<Animator>();
        wall = LayerMask.GetMask("Wall");
        platform = LayerMask.GetMask("Platform");
    }
    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        anim.speed = timeController.getTimeSpeed();

        if (isChasing)
        {
            anim.SetBool("isAttacking", true);
            AudioController.Instance.playFXSound(tenguAttack);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }

        hitWall = Physics2D.Raycast(transform.position, transform.forward, 0.5f, wall);
        hitPlatform = Physics2D.Raycast(transform.position, transform.forward, 0.5f, platform);

        if(hitWall || hitPlatform)
        {
            if (movesRight == true)
            {
                movesRight = false;
            }
            else
            {
                movesRight = true;
            }
        }
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        AudioController.Instance.playFXSound(tenguHit);
        base.TakeDamage(damage, direction);
    }
}

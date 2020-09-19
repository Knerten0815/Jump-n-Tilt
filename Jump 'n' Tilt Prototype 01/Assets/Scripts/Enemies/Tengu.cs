using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioControlling;

//Author: Nicole Mynarek
public class Tengu : FlyingEnemy
{
    private bool hitWall;               //true if wall is hit
    private bool hitPlatform;           //true if platform is hit
    private LayerMask wall;             // layer for detecting what a wall is
    private LayerMask platform;         // layer for detecting what a platform is

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

        //animations change regarding of FlyingEnemy's state
        if (isChasing)
        {
            if (!anim.GetBool("isAttacking"))
                AudioController.Instance.playFXSound(tenguAttack);

            anim.SetBool("isAttacking", true);
        }
        else
        {
            anim.SetBool("isAttacking", false);
        }

        //checking for collision with walls or platforms to change the moving direction
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

    //overridden TakeDamage funciton to set sound effects
    public override void TakeDamage(int damage, Vector2 direction)
    {
        AudioController.Instance.playFXSound(tenguHit);
        base.TakeDamage(damage, direction);
    }
}

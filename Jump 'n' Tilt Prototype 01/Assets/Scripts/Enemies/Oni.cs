//Author: Kevin Zielke
using AudioControlling;
using UnityEngine;

/// <summary>
/// The blue enemies of Level 2, that patrol left and right on the ground. If the player is at the same height level as them,
/// they charge towards him.
/// </summary>
public class Oni : GroundEnemy
{
    [SerializeField] Audio oniAttack;       // sound when Oni attacks
    [SerializeField] Audio oniHit;          // sound when Oni gets hit
    [SerializeField] float attackSpeed;     // speed with which the Oni attacks

    private float speed;                    // help variable to set back walking speed

    protected override void Start()
    {
        base.Start();
        speed = moveSpeed;
        anim = GetComponent<Animator>();        
        attackRadius = 1.25f;
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        //slide when you should slide!
        if (isSliding)
        {
            Slide();
            anim.SetBool("isAttacking", false);
            anim.SetBool("isSliding", true);
        }
        //charge towards the player if he is on the same height and in reach
        else if (playerDirection().y > -cc2d.bounds.extents.y && playerDirection().y < cc2d.bounds.extents.y && Mathf.Abs(playerDirection().x) < 15f  && !hasAttacked)
        {
            if (playerDirection().x < 0)
                direction = -1;
            else if (playerDirection().x > 0)
                direction = 1;

            moveSpeed = attackSpeed;

            if (!anim.GetBool("isAttacking"))                
                AudioController.Instance.playFXSound(oniAttack);
            
            anim.SetBool("isAttacking", true);
            anim.SetBool("isSliding", false);
        }
        else
        {
            //patrol (idle)
            moveSpeed = speed;

            if (IsWallAhead() == true || isGroundAhead() == false)
            {
                if (isFacingRight == true)
                {
                    direction = -1;
                    isFacingRight = false;
                }
                else
                {
                    direction = 1;
                    isFacingRight = true;
                }
            }

            anim.SetBool("isAttacking", false);
            anim.SetBool("isSliding", false);
        }

        if (!hasAttacked)
            Movement(direction);
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        base.TakeDamage(damage, direction);
        AudioController.Instance.playFXSound(oniHit);
    }
}
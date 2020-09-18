using UnityEngine;
using AudioControlling;

/// <summary>
/// The human enemies of level 3, that follow the player as soon as they spot him. If the player is at higher
/// ground level as them, they try to reach the player by jumping. If the player is directly above them, they
/// stand still and await the next move.
/// </summary>
public class HumanEnemy : GroundEnemy
{
    [SerializeField] Audio humanAttack;             // sound when the human attacks
    [SerializeField] Audio humanHit;                // sound when the human gets hit
    [SerializeField] float eyeSightDistance;        // when the player is within this vertical distance, humans attack

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        isSliding = false;
    }

    protected override void Update()
    {
        base.Update();

        if (grounded)
            anim.SetBool("isJumping", false);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("human_runningAttack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            anim.SetBool("isAttacking", false);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("human_jumpAttack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            anim.SetBool("isAttacking", false);
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        if (grounded)
            anim.SetBool("isJumping", false);

        //slide when you should slide!
        if (isSliding)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isSliding", true);
            Slide();
        }
        //follows the player, when he is in sight
        else if (Mathf.Abs(playerDirection().x) < eyeSightDistance && getPlayerScript().health > 0)
        {
            anim.SetBool("isSliding", false);
            anim.SetBool("isIdle", false);
            anim.SetBool("isRunning", true);

            if (playerDirection().x < -0.5f)
                direction = -1;
            else if (playerDirection().x > 0.5f)
                direction = 1;
            else
            {
                direction = 0;
                anim.SetBool("isRunning", false);
                anim.SetBool("isSliding", false);
                anim.SetBool("isIdle", true);
            }
             
            //try to get on the player platform
            if ((playerDirection().y > 2 && grounded && Mathf.Abs(playerDirection().x) > 0.5f) || (IsWallAhead() && grounded))
            {
                Jump();
                anim.SetBool("isIdle", false);
                anim.SetBool("isJumping", true);
            }

            if (!hasAttacked)
                Movement(direction);
        }
        //be idle on player death
        else
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isSliding", false);
            anim.SetBool("isIdle", true);
        }
    
    }
    protected override void Jump()
    {
        isSliding = false;
        velocity = new Vector2(playerDirection().normalized.x * 5, jumpHeight);
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        base.TakeDamage(damage, direction);
        AudioController.Instance.playFXSound(humanHit);
    }

    public override void groundEnemyAttack(Collider2D enemy, Vector2 dmgDirection2D)
    {
        if (!hasAttacked && !isSliding && getPlayerScript().health > 0)
        {
            anim.SetBool("isAttacking", true);
            AudioController.Instance.playFXSound(humanAttack);
        }
        base.groundEnemyAttack(enemy, dmgDirection2D);
    }
}

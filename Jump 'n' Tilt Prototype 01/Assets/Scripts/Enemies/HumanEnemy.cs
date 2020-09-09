using UnityEngine;
using AudioControlling;

public class HumanEnemy : GroundEnemy
{
    [SerializeField] Audio humanAttack;
    [SerializeField] Audio humanHit;
    [SerializeField] float eyeSightDistance;

    private Animator anim;

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

        //slide when you should slide!
        if (isSliding)
        {
            anim.SetBool("isJumping", false);
            Slide();
        }
        //follows the player, when he is in humans eyesight
        else if (Mathf.Abs(playerDirection().x) < eyeSightDistance && getPlayerScript().health > 0)
        {
            if (playerDirection().x < -0.5f)
                direction = -1;
            else if (playerDirection().x > 0.5f)
                direction = 1;
            else
                direction = 0;

            //try to get on the player platform
            if ((playerDirection().y > 2 && grounded && Mathf.Abs(playerDirection().x) > 0.1f) || (IsWallAhead() && grounded))
            {
                Jump();
                anim.SetBool("isJumping", true);
            }
            
            Movement(direction);            
        }
        else
        {
            //idle
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

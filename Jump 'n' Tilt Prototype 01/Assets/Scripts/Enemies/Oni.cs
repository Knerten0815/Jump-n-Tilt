using AudioControlling;
using UnityEngine;

public class Oni : GroundEnemy
{
    [SerializeField] Audio oniAttack;
    [SerializeField] Audio oniHit;
    [SerializeField] float attackSpeed;

    private Animator anim;
    private float speed;

    protected override void Start()
    {
        base.Start();
        speed = moveSpeed;
        anim = GetComponent<Animator>();
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        /* Charge check: when the player is in range and between these two rays, the oni will charge towards him
        Vector2 testorigin = transform.position;
        Vector2 testray = playerDirection();
        testray.y = 0f;
        Debug.DrawRay(testorigin, testray);
        testorigin.y -= cc2d.bounds.extents.y;
        Debug.DrawRay(testorigin, testray);
        */

        //slide when you should slide!
        if (isSliding)
        {
            Slide();
            anim.SetBool("isAttacking", false);
            anim.SetBool("isSliding", true);
        }
        //charge towards the player if he is on the same height and in reach
        else if (playerDirection().y < 0 && playerDirection().y > -cc2d.bounds.extents.y && Mathf.Abs(playerDirection().x) < 15f)
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
            //patrol
            moveSpeed = speed;
            if (IsWallAhead(false) == true || isGroundAhead(true) == false)
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
        Movement(direction);
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        base.TakeDamage(damage, direction);
        AudioController.Instance.playFXSound(oniHit);
    }
}
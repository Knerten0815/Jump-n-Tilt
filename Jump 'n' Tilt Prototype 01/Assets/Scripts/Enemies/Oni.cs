using AudioControlling;
using UnityEngine;

public class Oni : GroundEnemy
{
    [SerializeField] Audio oniAttack;
    [SerializeField] Audio oniHit;
    [SerializeField] float attackSpeed;
    private float speed;
    private bool isAttacking;

    protected override void Start()
    {
        base.Start();
        speed = moveSpeed;
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        Debug.DrawRay(transform.position, velocity);

        //slide when you should slide!
        if (isSliding)
        {
            isAttacking = false;
            Slide();
        }
        //charge towards the player if he is on the same height and in reach
        else if (Mathf.Abs(playerDirection().y) < 0.5f && Mathf.Abs(playerDirection().x) < 15f)
        {
            if (playerDirection().x < 0)
                direction = -1;
            else if (playerDirection().x > 0)
                direction = 1;

            moveSpeed = attackSpeed;
            if (!isAttacking)
            {
                isAttacking = true;
                AudioController.Instance.playFXSound(oniAttack);
            }                
        }
        else
        {
            isAttacking = false;
            //patrol
            moveSpeed = speed;
            if (IsWallAhead(saw) == true || isGroundAhead(sag) == false)
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
        }
        Movement(direction);
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        base.TakeDamage(damage, direction);
        AudioController.Instance.playFXSound(oniHit);
    }
}
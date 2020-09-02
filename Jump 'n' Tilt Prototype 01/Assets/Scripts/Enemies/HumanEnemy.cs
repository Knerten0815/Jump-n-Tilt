using UnityEngine;
using AudioControlling;

public class HumanEnemy : GroundEnemy
{
    [SerializeField] Audio humanAttack;
    [SerializeField] Audio humanHit;

    private Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        isSliding = false;
        //attackRadius = 1.25f;
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        //slide when you should slide!
        if (isSliding)
        {
            Debug.Log("slideridooo");
            Slide();
        }
        else if (Mathf.Abs(playerDirection().x) < 20f)
        {
            
            if (playerDirection().x < 0)
                direction = -1;
            else if (playerDirection().x > 0)
                direction = 1;

            if (playerDirection().y > 2 && grounded)
            {
                Jump();
            }                

            Movement(direction);            
        }
        else
        {
            Debug.Log("ich mach gar nix!");
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
}

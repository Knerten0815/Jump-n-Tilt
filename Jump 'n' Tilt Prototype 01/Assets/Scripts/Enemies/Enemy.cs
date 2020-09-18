//Author: Kevin Zielke
using UnityEngine;

/// <summary>
/// Base class for all enemies (GroundEnemy, FlyingEnemy). All enemies need information about the player and an animator.
/// </summary>
public class Enemy : Character
{
    protected GameObject player;
    public Animator anim;

    protected override void OnEnable()
    {
        base.OnEnable();
        whatIsEnemy = LayerMask.GetMask("Player");
        player = GameObject.Find("Player");
    }

    protected override void ComputeVelocity()
    {
        base.ComputeVelocity();

        if (isDead)
        {           
            gameObject.SetActive(false);
        }
    }

    protected override void updateAnimations()
    {
        anim.speed = timeController.getTimeSpeed();
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        Instantiate(bloodSpray, transform.position, Quaternion.identity);
        base.TakeDamage(damage, -direction);
    }

    //Author: Kevin Zielke
    /// <summary>
    /// Returns a Vector2 pointing to the transform of the player. Useful for AI.
    /// </summary>
    public Vector2 playerDirection()
    {
        return player.transform.position - transform.position;
    }

    //Author: Kevin Zielke
    /// <summary>
    /// Returns the PlayerCharacter Object. Useful for determining status of the player such as health, sliding, etc...
    /// </summary>
    /// <returns></returns>
    public PlayerCharacter getPlayerScript()
    {
        return player.GetComponent<PlayerCharacter>();
    }
}

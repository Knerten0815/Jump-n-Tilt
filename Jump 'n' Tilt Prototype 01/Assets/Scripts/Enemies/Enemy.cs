using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //Debug.Log("takeDamege von Enemy!!!!!!!!!");
    }

    /// <summary>
    /// Returns a Vector2 pointing to the transform of the player.
    /// </summary>
    public Vector2 playerDirection()
    {
        return player.transform.position - transform.position;
    }

    public PlayerCharacter getPlayerScript()
    {
        return player.GetComponent<PlayerCharacter>();
    }

}

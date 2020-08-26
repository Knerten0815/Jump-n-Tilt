using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    protected override void OnEnable()
    {
        base.OnEnable();
        whatIsEnemy = LayerMask.GetMask("Player");
    }

    public override void TakeDamage(int damage, Vector2 direction)
    {
        base.TakeDamage(damage, direction);
        Debug.Log("takeDamege von Enemy!!!!!!!!!");
    }
}

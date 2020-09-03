using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : PhysicsObject
{    // for Attack method
    public Transform attackPos;                 // is set in Unity window
    public float attackRadius;
    public LayerMask whatIsEnemy;
    protected virtual void Attack()
    {

        //  Debug.Log("Nicole ---------- ATTACK!!!!!!!");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);

        for (int i = 0; i < enemies.Length; i++)
        {

            Vector3 dmgDirection = gameObject.transform.localPosition - enemies[i].GetComponent<Transform>().localPosition;
            Vector2 dmgDirection2D = new Vector2(dmgDirection.x, dmgDirection.y);
            dmgDirection2D.Normalize();
            Character enemy = enemies[i].gameObject.GetComponent<Character>();
            if (enemy != null)
                enemies[i].GetComponent<Character>().TakeDamage(1, dmgDirection2D);
         
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        Attack();
    }
}

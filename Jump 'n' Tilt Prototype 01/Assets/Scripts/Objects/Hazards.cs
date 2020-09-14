using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazards : PhysicsObject
{    // for Attack method
    public Transform attackPos;                 // is set in Unity window
    public float attackRadius;
    public LayerMask whatIsEnemy;
    private bool hasDamaged =false;
    private Coroutine coolDamageRoutine;

    protected virtual void Attack()
    {
        if (!SpikeSafety.checkForSafety())
        {
            //  Debug.Log("Nicole ---------- ATTACK!!!!!!!");
          
            SpikeSafety.SetOnSafety();
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);

            for (int i = 0; i < enemies.Length; i++)
            {

                Vector3 dmgDirection = gameObject.transform.localPosition - enemies[i].GetComponent<Transform>().localPosition;
                Vector2 dmgDirection2D = new Vector2(dmgDirection.x, dmgDirection.y);
                dmgDirection2D.Normalize();
                Character enemy = enemies[i].gameObject.GetComponent<Character>();
                if (enemy != null && enemy.health > 0)
                    enemies[i].GetComponent<Character>().TakeDamage(1, dmgDirection2D);

            }
            //coolDamageRoutine = StartCoroutine(damageCooldown(2.0f));

        }
    }
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCharacter isPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        if (isPlayer != null)
        {
            Debug.Log("hit");
            Attack();
        }
    }
    IEnumerator damageCooldown(float coolDownTime)
    {
        //Debug.Log(coolDownTime + " seconds Cooldown!");
        yield return new WaitForSeconds(coolDownTime);
        //Debug.Log("Cooldown end!");
        hasDamaged = false;
        StopCoroutine(coolDamageRoutine);
    }
}

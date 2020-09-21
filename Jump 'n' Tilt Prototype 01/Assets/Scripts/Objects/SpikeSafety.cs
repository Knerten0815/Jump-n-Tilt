using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Katja Tuemmers
//SpikeSafety is a singleton. Any spike when attacking the player will set the boolean 
//hasDamaged to true which is checked by all other spikes before harming the player again
//when the spike safety is set on true, a coroutine is started that sets it back to false
//so the player can get hurt again after awhile
public class SpikeSafety : MonoBehaviour
{
    public static SpikeSafety _instance;
    private bool hasDamaged = false;
    private Coroutine coolDamageRoutine;
    void Awake()
    { 

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    public void SetOnSafety()
    {
        hasDamaged = true;
        coolDamageRoutine = StartCoroutine(damageCooldown(1.0f));
    }
  
    
    public bool checkForSafety()
    {
        return hasDamaged;
    }



    IEnumerator damageCooldown(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        hasDamaged = false;
        StopCoroutine(coolDamageRoutine);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

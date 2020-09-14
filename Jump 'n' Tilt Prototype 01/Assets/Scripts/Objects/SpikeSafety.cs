using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSafety : MonoBehaviour
{
    public static SpikeSafety instance; //the instance of our class that will do the work
    private static bool hasDamaged = false;
    private static Coroutine coolDamageRoutine;
    void Awake()
    { //called when an instance awakes in the game
        instance = this; //set our static reference to our newly initialized instance
    }

    public static void SetOnSafety()
    {
        hasDamaged = true;
        coolDamageRoutine = instance.StartCoroutine(damageCooldown(1.0f));
    }
  
    
    public static bool checkForSafety()
    {
        return hasDamaged;
    }



    static IEnumerator damageCooldown(float coolDownTime)
    {
        //Debug.Log(coolDownTime + " seconds Cooldown!");
        yield return new WaitForSeconds(coolDownTime);
        //Debug.Log("Cooldown end!");
        hasDamaged = false;
        instance.StopCoroutine(coolDamageRoutine);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Katja Tuemmers
//Class works between the Kitsune and the EndBoss script. It starts the boss fight after being 
//notified by EndBoss by exchanging the place holder animated kitsune sprite with the real kitsune object by activating the
//the latter and deactivating the former
//It passed on health information from the Kitsune to EndBoss
public class EndBossContainer : MonoBehaviour
{
    [SerializeField]
    private GameObject KitsuneFake;

    [SerializeField]
    private GameObject KitsuneReal;
    [SerializeField]
    private EndBoss endBoss;


    private void OnEnable()
    {
        KitsuneReal.SetActive(false);
        endBoss.startUpEndboss += startEndBoss;

    }
    private void startEndBoss()
    {
        KitsuneFake.SetActive(false);
        KitsuneReal.SetActive(true);
    }
    public void passOnHealth(int health)
    {
        endBoss.passOnHealth(health);
    }
   

}

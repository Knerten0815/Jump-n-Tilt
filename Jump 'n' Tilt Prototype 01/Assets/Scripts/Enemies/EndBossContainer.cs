using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBossContainer : MonoBehaviour
{
    // Start is called before the first frame update
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

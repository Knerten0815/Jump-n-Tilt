using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;

//Author: Katja Tuemmers
public class SpawnArrowBoss : SpawnArrows
{
    [SerializeField]
    private GameObject sparkle;
    // Update is called once per frame
    public override void OnEnable()
    {
        Instantiate(sparkle, transform.position, Quaternion.identity);
        base.OnEnable();
    }
    private void OnDisable()
    {
        Instantiate(sparkle, transform.position, Quaternion.identity);
        
    }
}

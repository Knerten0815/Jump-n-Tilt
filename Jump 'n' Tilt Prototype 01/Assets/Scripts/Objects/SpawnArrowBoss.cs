using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;


public class SpawnArrowBoss : SpawnArrows
{
    [SerializeField]
    private GameObject sparkle;
    // Update is called once per frame
    public override void OnEnable()
    {
        Instantiate(sparkle, transform.position, Quaternion.identity);
        shoot = true;
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }
}

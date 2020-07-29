using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;

public class SpawnArrows : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float spawnTime = 3.0f;

    private TimeController timeController;
    private float timeUntilSpawn;

    private GameObject arrow;

    private void OnEnable()
    {
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShootingArrows());
    }

    //Spawn the arrow prefab at the position of this gameobject, normally it would be the dispenser for the arrows
    private void SpawnArrow()
    {
        arrow = Instantiate(arrowPrefab) as GameObject;
        //arrow.transform.parent = transform;
        arrow.transform.position = transform.position;
        //arrow.transform.position = new Vector3(transform.position.x, transform.position.y + 3f, transform.position.z);
        
    }

    IEnumerator ShootingArrows()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnArrow();
        }
    }
}

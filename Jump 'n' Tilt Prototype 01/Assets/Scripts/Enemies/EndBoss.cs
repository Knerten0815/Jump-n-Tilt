using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndBoss : MonoBehaviour
{
    // Start is called before the first frame update
    
    private int healthLost = 0;
    private BoxCollider2D triggerStart;
    [SerializeField]
    private GameObject HealthBar;
    [SerializeField]
    private GameObject[] hearts;
    [SerializeField]
    private GameObject[] ArrowWave1;
    private bool wave1On = false;
    [SerializeField]
    private GameObject[] ArrowWave2;
    private bool wave2On = false;
    [SerializeField]
    private GameObject[] ArrowWave3;
    private bool wave3On = false;

    [SerializeField]
    private Audio heartAppear;

    private Coroutine[] heartAppearRoutine = new Coroutine[8];
    private void OnEnable()
    {
        HealthBar.SetActive(false);
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }
        foreach (GameObject spawnObject in ArrowWave1)
        {
            SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
            spawnArrow.shoot = false;
        }
        foreach (GameObject spawnObject in ArrowWave2)
        {
            SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
            spawnArrow.shoot = false;
        }
        foreach (GameObject spawnObject in ArrowWave3)
        {
            SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
            spawnArrow.shoot = false;
        }
    
    }

    public delegate void notifyStart();
    public event notifyStart startUpEndboss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthBar.SetActive(true);
        Debug.Log("hallo how");
        int i = 0;
        AudioController.Instance.playFXSound(heartAppear);

        foreach (GameObject heart in hearts)
        {
            heartAppearRoutine[i] = StartCoroutine(displayHealth(i));
            i++; 
        }
    }

    IEnumerator displayHealth(int i)
    {
        Debug.Log("hallo");      
        Debug.Log("cooler effect");
        yield return new WaitForSeconds(0.27f*i);
        hearts[7-i].SetActive(true);
        Debug.Log("cool effect");
        AudioController.Instance.playFXSound(heartAppear);
        heartAppear.volume = heartAppear.volume * 1.3f;
        if (i == 7)
        {
            startUpEndboss();

        }


        StopCoroutine(heartAppearRoutine[i]);
   
    }
    public void passOnHealth()
    {
        hearts[healthLost].SetActive(false);
        if (healthLost > 1 && !wave1On)
        {
            foreach (GameObject spawnObject in ArrowWave1)
            {
                SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
                spawnArrow.shoot = true;

            }
            wave1On = true;
        }
        if (healthLost > 3 && !wave2On)
        {
            foreach (GameObject spawnObject in ArrowWave2)
            {
                SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
                spawnArrow.shoot = true;
            }
            wave2On = true;
        }
        if (healthLost > 5 && !wave3On)
        {
            foreach (GameObject spawnObject in ArrowWave3)
            {
                SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
                spawnArrow.shoot = true;
            }
            wave3On = true;
        }

        healthLost++;
        
    }
}

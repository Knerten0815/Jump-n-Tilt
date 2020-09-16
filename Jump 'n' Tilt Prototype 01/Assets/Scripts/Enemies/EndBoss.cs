using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

using TMPro;


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

    private Coroutine deathWaitRoutine;
    [SerializeField]
    private Dialogue dialogue;
    private EventSystem m_EventSystem;

    private Coroutine[] heartAppearRoutine = new Coroutine[8];
    private void OnEnable()
    {
        triggerStart = this.GetComponent<BoxCollider2D>();
        HealthBar.SetActive(false);
        foreach (GameObject heart in hearts)
        {
            heart.SetActive(false);
        }
        foreach (GameObject spawnObject in ArrowWave1)
        {
            spawnObject.SetActive(false);
            SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
            spawnArrow.shoot = false;

        }
        foreach (GameObject spawnObject in ArrowWave2)
        {
            spawnObject.SetActive(false);
            SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
            spawnArrow.shoot = false;
        }
        foreach (GameObject spawnObject in ArrowWave3)
        {
            spawnObject.SetActive(false);
            SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
            spawnArrow.shoot = false;
        }
    
    }

    public delegate void notifyStart();
    public event notifyStart startUpEndboss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HealthBar.SetActive(true);
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
   
        yield return new WaitForSeconds(0.27f*i);
        hearts[7-i].SetActive(true);

        AudioController.Instance.playFXSound(heartAppear);
        heartAppear.volume = heartAppear.volume * 1.3f;
        if (i == 7)
        {
            startUpEndboss();
            Object.Destroy(triggerStart);

        }


        StopCoroutine(heartAppearRoutine[i]);
   
    }
    public void passOnHealth(int health)
    {
        hearts[healthLost].SetActive(false);
        if (health == 0)
        {
            HealthBar.SetActive(false);
            foreach (GameObject spawnObject in ArrowWave1)
            {
                spawnObject.SetActive(false);
            }
            foreach (GameObject spawnObject in ArrowWave2)
            {
                spawnObject.SetActive(false);
            }
            foreach (GameObject spawnObject in ArrowWave3)
            {
                spawnObject.SetActive(false);
            }
            deathWaitRoutine = StartCoroutine(waitForDeath(2.0f));

        }
        if (healthLost > 1 && !wave1On)
        {
            foreach (GameObject spawnObject in ArrowWave1)
            {
                spawnObject.SetActive(true);
                SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
                spawnArrow.shoot = true;

            }
            wave1On = true;
        }
        if (healthLost > 3 && !wave2On)
        {
            foreach (GameObject spawnObject in ArrowWave2)
            {
                spawnObject.SetActive(true);
                SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
                spawnArrow.shoot = true;
            }
            wave2On = true;
        }
        if (healthLost > 5 && !wave3On)
        {
            foreach (GameObject spawnObject in ArrowWave3)
            {
                spawnObject.SetActive(true);
                SpawnArrows spawnArrow = spawnObject.GetComponent<SpawnArrows>();
                spawnArrow.shoot = true;
            }
            wave3On = true;
        }

        healthLost++;
        
    }
    private void finalDialogue()
    {
        DialogueManager.Instance.setLastDialogue(true);
        DialogueManager.Instance.StartDialogue(dialogue);
        m_EventSystem = EventSystem.current;
        m_EventSystem.SetSelectedGameObject(DialogueManager.Instance.button);
        DialogueManager.Instance.button.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
        DialogueManager.Instance.button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);

    }
    IEnumerator waitForDeath(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        //Debug.Log("Cooldown end!");
        finalDialogue();
        StopCoroutine(deathWaitRoutine);
    }
}

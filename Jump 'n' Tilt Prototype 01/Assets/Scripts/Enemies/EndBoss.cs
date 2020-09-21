using AudioControlling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

using TMPro;

//Author: Katja Tuemmers
//Class that controls the framework of the boss fight
//Has collider to trigger the beginning of the boss fight and starts the last Dialouge after the
//the Kitsune is defeated. It notifies EndBossContianer to activate the real Kitsune
//Depending on its health it spawns arrow traps 
//It displays and modifies the healthbar of the Kitsune with information about the Kitsunes health 
//that is being passed on by the EndBossContainer
//It adds sound effects to the Kitsune health display bar
//

public class EndBoss : MonoBehaviour
{    
    private int healthLost = 0;
    private BoxCollider2D triggerStart;
    [SerializeField]
    private GameObject HealthBar;
    [SerializeField]
    private GameObject[] hearts;


    /*
    *
    * Following arrays contain the dispenser prefabs for each wave of newly spawn arrow
    * trapps that should be triggered at different health loss of the Kitsune
    * 
    *
    */
    [SerializeField]
    private GameObject[] ArrowWave1;
    private bool wave1On = false;
    [SerializeField]
    private GameObject[] ArrowWave2;
    private bool wave2On = false;
    [SerializeField]
    private GameObject[] ArrowWave3;
    private bool wave3On = false;

    //Audio for the heartAppear sound effect
    [SerializeField]
    private Audio heartAppear;

    //Coroutine to make the Health of the Kitsune appear with little wait times to give it gravitas
    private Coroutine[] heartAppearRoutine = new Coroutine[8];

    //After the kitsune dies there is a wait time before starting the finale dialouge
    private Coroutine deathWaitRoutine;

    //Final dialouge
    [SerializeField]
    private Dialogue dialogue;
    private EventSystem m_EventSystem;

 /*
 * When first enabled its made sure all arrows are deactivated
 */
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

    //event to notify the start of the boss fight
    public delegate void notifyStart();
    public event notifyStart startUpEndboss;

    //when player enters the collider to trigger the boss fight the blank health bar appears
    //then with the help of the heartAppearRoutine each heart appears with a bang after one another
    //when the last heart is displayed the startUpEndBoss event is thrown
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
    //activates the hearts with sound that gets louder each time and throws the event for the start of the 
    //boss fight when the last heart is activated
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

    //EndBoss receives health of the Kitsune and activates the dispenser after a certain loss
    //when the kitsune dies all dispensers are deactivated and the deathWaitRoutine is started
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

    //starts the finaleDialouge by setting lastdialouge in the DialogueManager to true and then
    //calls for StartDialouge() similar to the Dialogue Trigger Objects

    private void finalDialogue()
    {
        DialogueManager.Instance.setLastDialogue(true);
        DialogueManager.Instance.StartDialogue(dialogue);
        m_EventSystem = EventSystem.current;
        m_EventSystem.SetSelectedGameObject(DialogueManager.Instance.button);
        DialogueManager.Instance.button.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
        DialogueManager.Instance.button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);

    }

    //After the death of the kitsune this routine is started and after a short time the final dialouge is started
    IEnumerator waitForDeath(float coolDownTime)
    {
        yield return new WaitForSeconds(coolDownTime);
        //Debug.Log("Cooldown end!");
        finalDialogue();
        StopCoroutine(deathWaitRoutine);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//Author: Michelle Limbach
//This class handels the different dialogue triggers
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; //Dialogue element of the current dialogue trigger game object
    EventSystem m_EventSystem; //Current event system
    private bool notTriggeredYet = true; //Variable to check if this dialogue was triggered before

   //Method to Trigger a Dialogue
    public void TriggerDialogue()
    {
        if (notTriggeredYet) //Checks if this dialogue was not triggered before
        {

            DialogueManager.Instance.StartDialogue(dialogue); //Call startDialogue method with current dialogue
            m_EventSystem = EventSystem.current; //Get current event system
            m_EventSystem.SetSelectedGameObject(DialogueManager.Instance.button); //Set button of dialogue box to selected game object in the event system
            DialogueManager.Instance.button.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold; //Change font style of button
            DialogueManager.Instance.button.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f); //Change color of button
            notTriggeredYet = false;
        }
    }

    //Method is called when a collider enters the collider of the game objects this script is attached to
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player") //Checks if the triggering collider is the player collider
        {
            GetComponent<DialogueTrigger>().TriggerDialogue(); //If true then start the dialogue
        }
    }
}

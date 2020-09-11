using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//Author: Michelle Limbach
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public GameObject continueButton;
    EventSystem m_EventSystem;
    private bool notTriggeredYet = true;

    private void OnEnable()
    {
        continueButton = DialogueManager.Instance.button;
    }
    public void TriggerDialogue()
    {
        if (notTriggeredYet)
        {

            DialogueManager.Instance.StartDialogue(dialogue);
            m_EventSystem = EventSystem.current;
            m_EventSystem.SetSelectedGameObject(continueButton);
            continueButton.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
            continueButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);
            notTriggeredYet = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == "Player")
        {
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }
}

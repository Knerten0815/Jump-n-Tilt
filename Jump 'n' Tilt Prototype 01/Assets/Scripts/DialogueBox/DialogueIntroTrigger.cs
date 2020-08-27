using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIntroTrigger : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<DialogueTrigger>().TriggerDialogue();
    }
}

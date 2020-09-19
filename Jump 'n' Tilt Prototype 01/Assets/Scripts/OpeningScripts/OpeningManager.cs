using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//Author: Michelle Limbach
//This script handels the display of the text in the Opening scene
public class OpeningManager : MonoBehaviour
{

    private Queue<string> sentences; //Queue that later contains the sentences that should be displayed
    public TextMeshProUGUI openingText; //GUI element where the text should be displayed
    public Opening opening; //Opening element that contains the sentences from the inspector
    public GameObject continueButton; //Reference to the button in the scene so it can be easily styled and accessed via script
    EventSystem m_EventSystem; //current eventsystem

   
    void Awake()
    {
        sentences = new Queue<string>(); //new empty Queue gets created
    }

    private void Start()
    {
        StartOpening(opening); //The opening object from the inspector with all sentences is given to the StartOpening method
        m_EventSystem = EventSystem.current; //Get the current eventsystem of this scene
        m_EventSystem.SetSelectedGameObject(continueButton); //Set the continue button to the selected gameobject in the current eventsystem so controller, keyboard and mouse control is possible
        continueButton.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold; //Change font style of button
        continueButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f); //Change color of button
    }

    //This method saves the sentences from the opening object in a queue and starts the first paragraph
    public void StartOpening(Opening opening)
    {
        sentences.Clear(); //Clears the queue
        foreach (string paragraph in opening.paragraphs)
        {
            sentences.Enqueue(paragraph); //Enqueues every paragraph of the opening object
        }

        DisplayNextParagraph(); //Starts first paragraph
    }

    //This method displays senquentially the paragraphs of the opening object
    public void DisplayNextParagraph()
    {
        //If there are no sentences left, then end the opening scene
        if (sentences.Count == 0)
        {
            EndOpening();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence)); 
    }

    //Coroutine to create a typing illusion for the user
    IEnumerator TypeSentence(string sentence)
    {
        openingText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            openingText.text += letter;
            yield return null;
        }
    }

    //Ends the Opening scene and loads the Intro scene
    public void EndOpening()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(5);
    }
}

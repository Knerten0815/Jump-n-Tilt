using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//Author: Michelle Limbach
//This class handels the display of the Dialogue Boxes
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    
    //Queues for names and sentences
    private Queue<string> sentences;
    private Queue<string> names;

    //Button of the dialogue box
    public GameObject button;

    //GUI elements for the name and the text
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    //GUI image element for the dialogue box
    public GameObject dialogueImage;

    //All images for the speakers
    private Sprite[] dialogueImages;

    //Animator; is a different one for Intro and other scenes
    public Animator animator;
    private RuntimeAnimatorController introController;
    private RuntimeAnimatorController animatorController;

   
    private bool lastDialogue;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Create new Queues for names and sentences
        sentences = new Queue<string>();
        names = new Queue<string>();

        //Load all five images of the different speakers from the resources
        dialogueImages = new Sprite[5];
        dialogueImages[0] = Resources.Load<Sprite>("DialogueImages/Player_64x64_ver2");
        dialogueImages[1] = Resources.Load<Sprite>("DialogueImages/onamazu64x64"); 
        dialogueImages[2] = Resources.Load<Sprite>("DialogueImages/kitsune64x64"); 
        dialogueImages[3] = Resources.Load<Sprite>("DialogueImages/Fragezeichen"); 
        dialogueImages[4] = Resources.Load<Sprite>("DialogueImages/Fragezeichen");

        //Load the intro and animator controller
        introController = Resources.Load("AnimatorForDialogue/DialogueBoxIntro") as RuntimeAnimatorController;
        animatorController = Resources.Load("AnimatorForDialogue/DialogueBox") as RuntimeAnimatorController;

        //Subscribe to sceneLoaded event
        SceneManager.sceneLoaded += findNewCamera;
        SceneManager.sceneLoaded += setAnimator;

    }
    //Unsubscribe to sceneLoaded event
    private void OnDisabled()
    {
        SceneManager.sceneLoaded -= findNewCamera;
        SceneManager.sceneLoaded -= setAnimator;
    }
    //Function to get the main camera when a new scene is loaded.
    //Change renderMode, worldCamera, sortingLayer and sortingOrder of the dialogue canvas element
    private void findNewCamera(Scene aScene, LoadSceneMode aMode)
    {
        this.GetComponentInChildren<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        this.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        this.GetComponentInChildren<Canvas>().sortingLayerName = "PickUpLayer";
        this.GetComponentInChildren<Canvas>().sortingOrder = 5;
    }

    //Function to set the Animator when a new scene is loaded
    private void setAnimator(Scene aScene, LoadSceneMode aMode)
    {
        //Get dialogueBox gameObject
        GameObject dialogueBox = this.GetComponentInChildren<Animator>().gameObject;
        //Check if current scene is Intro scene
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            //Set position of dialogueBox in the center of screen
            dialogueBox.transform.localPosition = new Vector3(0f, dialogueBox.transform.position.y, dialogueBox.transform.position.z);
            //Set Animator controller to introController
            this.GetComponentInChildren<Animator>().runtimeAnimatorController = introController;
            
        }
        else
        {
            //Set position of dialogueBox to upper left corner
            dialogueBox.transform.localPosition = new Vector3(-305f, dialogueBox.transform.position.y, dialogueBox.transform.position.z);
            //Set Animator controller
            this.GetComponentInChildren<Animator>().runtimeAnimatorController = animatorController;
        }
        
        
    }

    //Function to start a given dialogue
    public void StartDialogue(Dialogue dialogue)
    {
        //Open dialogue Box
        animator.SetBool("isOpen", true);
        //Clear queues
        sentences.Clear();
        names.Clear();
        //Disable player controls
        Time.timeScale = 0f;

        //Disable old speaker image
        dialogueImage.SetActive(false);
        
        //Fill queues with given dialogue names and sentences
        foreach (string name in dialogue.name)
        {
            names.Enqueue(name);
        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        //Display first Sentence  
        DisplayNextSectence();
    }

    //Function to show the next sentence
    public void DisplayNextSectence()
    {
        dialogueImage.SetActive(true);

        //Check if the sentences queue is empty and lastDialogue is true
        //Called after boss fight
        if (sentences.Count  == 0 && lastDialogue)
        {
            lastDialogue = false;
            Time.timeScale = 1f; //Enable player control
            EndDialogue();
            ManagementSystem.Instance.endLevel();

            return;
        }
        //Check if sentences queue is empty, not currently in Intro scene and lastDialogue false
        else if (sentences.Count == 0 && SceneManager.GetActiveScene().name != "Intro" && !lastDialogue)
        {
            EndDialogue(); //Just end dialogue during game play
            return;
        }
        //Check if sentences queue is empty, currently in Intro scene and lastDialogue false
        //Called at end of Intro scene
        else if(sentences.Count == 0 && SceneManager.GetActiveScene().name == "Intro" && !lastDialogue)
        {
            Time.timeScale = 1f; //Enable player control
            UnityEngine.SceneManagement.SceneManager.LoadScene(6); //Load first level
            return;
        }
        

        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        nameText.text = name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
      
        //Display corresponding image to name of speaker
        switch (name)
        {
            case "Kashima":

                dialogueImage.GetComponent<Image>().sprite = dialogueImages[0];
                break;
            case "Onamazu":
               
                dialogueImage.GetComponent<Image>().sprite = dialogueImages[1];
                break;
            case "Kitsune":
                
                dialogueImage.GetComponent<Image>().sprite = dialogueImages[2];
                break;
            case "Tutorial":
                
                dialogueImage.GetComponent<Image>().sprite = dialogueImages[3];
                break;
            case "Narrator":
                
                dialogueImage.GetComponent<Image>().sprite = dialogueImages[4];
                break;
        }
    }
    //Coroutine to create a typing illusion for sentences
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    //Setter method for lastDialogue variable
    public void setLastDialogue(bool lastDialogue)
    {
        this.lastDialogue = lastDialogue;
    }

    //Function to end a dialogue
    //Starts the closing animation of the dialogue box and after that disables the dialogue box
    public void EndDialogue()
    {
        Time.timeScale = 1f; // Enable player control
        animator.SetBool("isOpen", false);
        dialogueImage.SetActive(false);
    }

}

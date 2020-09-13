using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//Author: Michelle Limbach
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private Queue<string> sentences;
    private Queue<string> names;
    public GameObject button;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueImage;
    private Sprite[] dialogueImages;
    public Animator animator;
    private RuntimeAnimatorController introController;
    private RuntimeAnimatorController animatorController;


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

        sentences = new Queue<string>();
        names = new Queue<string>();
        dialogueImages = new Sprite[5];
        dialogueImages[0] = Resources.Load<Sprite>("DialogueImages/Player_64x64_ver2");
        dialogueImages[1] = Resources.Load<Sprite>("DialogueImages/onamazu64x64"); 
        dialogueImages[2] = Resources.Load<Sprite>("DialogueImages/kitsune64x64"); 
        dialogueImages[3] = Resources.Load<Sprite>("DialogueImages/Fragezeichen"); 
        dialogueImages[4] = Resources.Load<Sprite>("DialogueImages/Fragezeichen");

        introController = Resources.Load("AnimatorForDialogue/DialogueBoxIntro") as RuntimeAnimatorController;
        animatorController = Resources.Load("AnimatorForDialogue/DialogueBox") as RuntimeAnimatorController;

        SceneManager.sceneLoaded += findNewCamera;
        SceneManager.sceneLoaded += setAnimator;

    }
    private void OnDisabled()
    {
        SceneManager.sceneLoaded -= findNewCamera;
        SceneManager.sceneLoaded -= setAnimator;
    }

    private void findNewCamera(Scene aScene, LoadSceneMode aMode)

    {
        this.GetComponentInChildren<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        this.GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }
    private void setAnimator(Scene aScene, LoadSceneMode aMode)
    {
        GameObject dialogueBox = this.GetComponentInChildren<Animator>().gameObject;
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            
            dialogueBox.transform.localPosition = new Vector3(0f, dialogueBox.transform.position.y, dialogueBox.transform.position.z);
            this.GetComponentInChildren<Animator>().runtimeAnimatorController = introController;
            
        }
        else
        {
            dialogueBox.transform.localPosition = new Vector3(-305f, dialogueBox.transform.position.y, dialogueBox.transform.position.z);
            this.GetComponentInChildren<Animator>().runtimeAnimatorController = animatorController;
        }
        
        
    }

    public void StartDialogue(Dialogue dialogue)
    {

        animator.SetBool("isOpen", true);
        sentences.Clear();
        names.Clear();
        Time.timeScale = 0f;


        dialogueImage.SetActive(false);
        
        foreach (string name in dialogue.name)
        {
            names.Enqueue(name);
        }

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSectence();
    }

    public void DisplayNextSectence()
    {
        dialogueImage.SetActive(true);
        if (sentences.Count == 0 && SceneManager.GetActiveScene().name != "Intro")
        {
            EndDialogue();
            return;
        }
        else if(sentences.Count == 0 && SceneManager.GetActiveScene().name == "Intro")
        {

            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(6);
            return;
        }

        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        nameText.text = name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
      
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

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        Time.timeScale = 1f;
        animator.SetBool("isOpen", false);
        dialogueImage.SetActive(false);
    }

}

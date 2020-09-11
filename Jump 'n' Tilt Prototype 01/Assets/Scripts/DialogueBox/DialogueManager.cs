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
    private GameObject[] dialogueImages;
    public Animator animator;
    private GameObject currentActiveImage;

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
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);
        sentences.Clear();
        names.Clear();
        Time.timeScale = 0f;
        if(dialogue.images.Length > 5)
        {
            Debug.Log("Bitte nicht mehr als 5 Bilder in den Dialoge laden!");
            EndDialogue();
            return;
        }
        dialogueImages = new GameObject[dialogue.images.Length];
        foreach (GameObject image in dialogue.images)
        {
            switch (image.name)
            {
                case "Kashima":
                    dialogueImages[0] = image;
                    break;
                case "Onamazu":
                    dialogueImages[1] = image;
                    break;
                case "Kitsune":
                    dialogueImages[2] = image;
                    break;
                case "Tutorial":
                    dialogueImages[3] = image;
                    break;
                case "Narrator":
                    dialogueImages[4] = image;
                    break;
            }
        }
        foreach(GameObject image in dialogueImages)
        {
            image.SetActive(false);
        }
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
        if (sentences.Count == 0 && SceneManager.GetActiveScene().name != "Intro")
        {
            EndDialogue();
            return;
        }
        else if(sentences.Count == 0 && SceneManager.GetActiveScene().name == "Intro")
        {
            SceneManager.LoadScene("Level 1");
            return;
        }

        string sentence = sentences.Dequeue();
        string name = names.Dequeue();
        nameText.text = name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if(currentActiveImage != null)
        {
            currentActiveImage.SetActive(false);
        }
        switch (name)
        {
            case "Kashima":
                dialogueImages[0].SetActive(true);
                currentActiveImage = dialogueImages[0];
                break;
            case "Onamazu":
                dialogueImages[1].SetActive(true);
                currentActiveImage = dialogueImages[1];
                break;
            case "Kitsune":
                dialogueImages[2].SetActive(true);
                currentActiveImage = dialogueImages[2];
                break;
            case "Tutorial":
                dialogueImages[3].SetActive(true);
                currentActiveImage = dialogueImages[3];
                break;
            case "Narrator":
                dialogueImages[4].SetActive(true);
                currentActiveImage = dialogueImages[4];
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
        if (currentActiveImage != null)
        {
            currentActiveImage.SetActive(false);
        }
    }

}

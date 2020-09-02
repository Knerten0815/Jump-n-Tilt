using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
//Author: Michelle Limbach
public class OpeningManager : MonoBehaviour
{
    public static OpeningManager Instance { get; private set; }
    private Queue<string> sentences;
    public TextMeshProUGUI openingText;
    public Opening opening;
    public GameObject continueButton;
    EventSystem m_EventSystem;

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
    }

    private void Start()
    {
        StartOpening(opening);
        m_EventSystem = EventSystem.current;
        m_EventSystem.SetSelectedGameObject(continueButton);
        continueButton.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
        continueButton.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);
    }
    public void StartOpening(Opening opening)
    {
        sentences.Clear();
        foreach (string paragraph in opening.paragraphs)
        {
            sentences.Enqueue(paragraph);
        }

        DisplayNextParagraph();
    }

    public void DisplayNextParagraph()
    {
        if (sentences.Count == 0)
        {
            EndOpening();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        openingText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            openingText.text += letter;
            yield return null;
        }
    }

    public void EndOpening()
    {
        //Load new scene
        SceneManager.LoadScene("Intro");
    }
}

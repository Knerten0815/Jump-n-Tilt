using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HighScoreScene : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI newHighscore;
    [SerializeField]
    private TextMeshProUGUI score;
    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button[] nextLevelButton;


    [SerializeField]
    private Button setName;

    [SerializeField]
    private GameObject enterNamePage;

    [SerializeField]
    private GameObject[] displayPage;

    private int newSpot;
    private int currentLevel;
    private int currentScore;

    // Start is called before the first frame update
    void Start()
    {
        var temp = ManagementSystem.checkForNewHighScore();
        newSpot = temp.Item1;
        currentLevel = temp.Item2;
        currentScore = temp.Item3;
        score.text = currentScore.ToString();
        Debug.Log("waht currentScore " + currentScore);

        if (newSpot == -1)
        {
            continueButton.onClick.AddListener(() => nextPage(displayPage[currentLevel]));
            newHighscore.text = "";
        }
        else
        {
            
            continueButton.onClick.AddListener(() => nextPage(enterNamePage));
            setName.onClick.AddListener(() => setNewHighscore());

        }
        Debug.Log("waht currentLevel " + currentLevel);
        nextLevelButton[currentLevel].onClick.AddListener(() => nextLevel());
    }

    private void nextPage(GameObject page)
    {
        page.SetActive(true);
    }

    
    private void displayScore(string name, int score, int level, int spot)
    {

    }

    private void setNewHighscore() 
    {
        TextInput tInput = enterNamePage.GetComponent<TextInput>();
        bool didItHappen = ManagementSystem.newHighScore(tInput.getInputText(), newSpot);
        displayPage[currentLevel].SetActive(true);


    }
    private void nextLevel()
    {
        //ManagementSystem.nextLevel();
    }
    
}

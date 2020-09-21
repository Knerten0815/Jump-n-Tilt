using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
*
* Author: Katja Tuemmers
* The HighScoreScene manages the scene of the same name and controls the flow of displayed pages in the menu. Instead of having a predefined
* flow this class checks whether a new high score was reached and changes the order of activation and deactivation of pages.
* When a new highscore was reached a notification appears under the total score and the next page prompts the user input their name
* With no new highscore the continue buttons redirects directly to the display of the current ranking.
* 
* The HighScoreScene also calls for the NextLevel() function in ManagementSystem that lets the player progress to the next stage and saves the game
* 
*
*/
public class HighScoreScene : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI newHighscore; //notification of the new highscore
    [SerializeField]
    private TextMeshProUGUI score; //reached score
    [SerializeField]
    private Button continueButton; //Button from the first page shown in the Highscore scene

    [SerializeField]
    private Button[] nextLevelButton; //Button to leave the highscore scene and progress to the next level


    [SerializeField]
    private Button setName; //button that sends the entered name 

    [SerializeField]
    private GameObject enterNamePage; //page with the option to enter name

    [SerializeField]
    private GameObject[] displayPage; //pages that display highscore rankings for each level

    private int newSpot; //what new spot in the ranking was reached
    private int currentLevel; //what level was it achieved in
    private int currentScore; //with what score

    // Start is called before the first frame update
    void Start()
    {
        var temp = ManagementSystem.Instance.checkForNewHighScore();//Checks for new high score
        newSpot = temp.Item1;
        currentLevel = temp.Item2;
        currentScore = temp.Item3;
        score.text = currentScore.ToString();
        if (newSpot == -1) //no new highscore reached, user will be shown the ranking display page onClick of the continue button on the first page
        {
            continueButton.onClick.AddListener(() => nextPage(displayPage[currentLevel]));
            newHighscore.text = ""; //notifcation is replaced by empty string
        }
        else
        {
            
            continueButton.onClick.AddListener(() => nextPage(enterNamePage)); //user will be redirected to enterNamePage
            //a listener is added to the setName button on the enterNamePage to read out the name entered by the Player
            setName.onClick.AddListener(() => setNewHighscore()); 

        }
        nextLevelButton[currentLevel].onClick.AddListener(() => nextLevel()); //button to leads to the next stage is assigned the nextLevel() function
    }
    //Next page is activated in the highscore scene and be varied by passing on a different page
    private void nextPage(GameObject page)
    {
        page.SetActive(true);
    }

    //Entered name by the player is retrieved and used to insert them in the highscore ranking with name and score
    private void setNewHighscore() 
    {
        TextInput tInput = enterNamePage.GetComponent<TextInput>();
        bool didItHappen = ManagementSystem.Instance.newHighScore(tInput.getInputText(), newSpot);
        displayPage[currentLevel].SetActive(true);


    }
    //calls for the next stage to be loaded
    private void nextLevel()
    {
        ManagementSystem.Instance.nextLevel();
    }
    
}

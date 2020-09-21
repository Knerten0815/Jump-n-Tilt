using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
/*
*
* Author: Katja Tuemmers
* Handles the Score. Remembers current score, increases it when an applicable event happens and also displays it, displays health,
* displays amount of time for slow down, displays collected scrolls in level
*       
*
* 
*/
public class ScoringSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText; //Text displayed to show the current Score
    public TextMeshProUGUI healthText; //Text displayed to show the current health
    public TextMeshProUGUI scrollText; //Text displayed to show the currently collected collectibles in a given level
    public TextMeshProUGUI timeText;//Text displayed to show the currently collected collectibles in a given level


    //ScoreSystem own tracking of values
    private int health;
    private int collected;
    private int timeMin;
    private int timeSec;
    public int Score; //Score of the player
    private string scoreString; //String to be displayed with scoreText


    /*
    * With Start scoreUp is subscribes to the necessary ManagementSystem events. It initializes the Score with 0 points, inserts it into the scoreString and sets the Text for the Text Component 
    * to the scoreString. Also initializes the current time with zero.
    * 
    *
    */
    public void Awake()
    {
        ManagementSystem.Instance.pickUpHit += scoreUp;
        // ManagementSystem.healthPickUpHit += healthBarUp;
        ManagementSystem.Instance.healthPassOn += healthDisplay;
        ManagementSystem.Instance.collectedScroll += updateScrollCount;
        ManagementSystem.Instance.timePassOn += displayTime;
        displayTime(0);
        Score = 0;
        collected = 0;
        timeMin = 0;
        timeSec = 0;
        scoreString = "0";
        scoreText.text = scoreString;

    }
    /*
    * Turns a float value representing seconds into the appropriate form to be displayed
    */
    private void displayTime(float time)
    {

        timeMin = (int)time / 60;

        timeSec = (int)time - timeMin * 60;

        string timeMinText = timeMin.ToString();
        if (timeMin < 10)
        {
            timeMinText = "0" + timeMin;
        }
        string timeSecText = timeSec.ToString();
        if (timeSec < 10)
        {
            timeSecText = "0" + timeSec;
        }

        timeText.text = timeMinText + ":" + timeSecText;
    }
    /*
     *
     * Increases Score and updates the Text Component
     * 
     *
     */
    private void scoreUp(int value)
    {
        Score += value;
        string scoreTempString = Score.ToString();
        string scoreString = scoreTempString;
        scoreText.text = scoreString;
    }
    /*
    * Updates the health value and display
    */
    private void healthDisplay(int health)
    {
        this.health = health;
        healthText.text = this.health.ToString();
    }
    /*
    * Updates the collectible count value and display
    */
    private void updateScrollCount()
    {
        collected++;
        scrollText.text = collected.ToString();
    }
  

}

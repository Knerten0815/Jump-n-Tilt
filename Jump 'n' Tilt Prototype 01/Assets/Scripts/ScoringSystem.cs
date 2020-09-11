using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
/*
*
* Hanldes the Score. Remembers current score, increases it when an applicable event happens and also displays it
*       
*
* @Katja
*/
public class ScoringSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText; //Text displayed to show the current Score
    public TextMeshProUGUI healthText; //Text displayed to show the current Score
    public TextMeshProUGUI scrollText; //Text displayed to show the current Score
    public TextMeshProUGUI timeText;



    private int health;
    private int collected;
    private int timeMin;
    private int timeSec;
    public int Score; //Score of the player
    private string scoreString; //String to be displayed with scoreText


    /*
    * With Start scoreUp is subscribed to pickUpHit. It initializes the Score with 0 points, inserts it into the scoreString and sets the Text for the Text Component 
    * to the scoreString
    * 
    * @Katja
    */
    public void Awake()
    {
        ManagementSystem.pickUpHit += scoreUp;
        // ManagementSystem.healthPickUpHit += healthBarUp;
        ManagementSystem.healthPassOn += healthDisplay;
        ManagementSystem.collectedScroll += updateScrollCount;
        ManagementSystem.timePassOn += displayTime;
        displayTime(0);
        Score = 0;
        collected = 0;
        timeMin = 0;
        timeSec = 0;
        scoreString = "0";
        scoreText.text = scoreString;

    }

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
     *@Katja
     */
    private void scoreUp(int value)
    {
        Score += value;
        string scoreTempString = Score.ToString();
       /* int scoreLength = scoreTempString.Length;
        string scoreString = "";
        int howManyZeros = 9 - scoreLength;
        while (howManyZeros > 0)
        {
            scoreString = scoreString + "0";

        }*/
        string scoreString = scoreTempString;
        scoreText.text = scoreString;
    }

    private void healthDisplay(int health)
    {
        this.health = health;
        healthText.text = this.health.ToString();
    }
    private void updateScrollCount()
    {
        collected++;
        scrollText.text = collected.ToString();
    }
  

}

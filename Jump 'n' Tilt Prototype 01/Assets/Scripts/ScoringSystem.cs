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
    private int health;
    private int collected;
    private int time; 
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
        Score = 0;
        collected = 0;
        time = 0;
        scoreString = "0";
        scoreText.text = scoreString;

    }

    /*
     *
     * Increases Score by 10 and updates the Text Component
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
        healthText.text = health.ToString();
    }
  

}

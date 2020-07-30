using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*
* Hanldes the Score. Remembers current score, increases it when an applicable event happens and also displays it
*       
*
* @Katja
*/
public class ScoringSystem : MonoBehaviour
{

    public GameObject scoreText; //Text displayed to show the current Score
    public int Score; //Score of the player
    private string scoreString; //String to be displayed with scoreText


    /*
    * With Start scoreUp is subscribed to pickUpHit. It initializes the Score with 0 points, inserts it into the scoreString and sets the Text for the Text Component 
    * to the scoreString
    * 
    * @Katja
    */
    public void Start()
    {
        ManagementSystem.pickUpHit += scoreUp;
        ManagementSystem.healthPickUpHit += healthBarUp;
        Score = 000000000;
        scoreString = Score.ToString();
        scoreText.GetComponent<UnityEngine.UI.Text>().text = scoreString;
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
        scoreString = Score.ToString();
        scoreText.GetComponent<UnityEngine.UI.Text>().text = scoreString;
    }

    private void healthBarUp()
    {
        /*
        *
        * Hier wäre dann das steigern des momentanen Lebens
        *
        *
        *
        *
        */
    }

}

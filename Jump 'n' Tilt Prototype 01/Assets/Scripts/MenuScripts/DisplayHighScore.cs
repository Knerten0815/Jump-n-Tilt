using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Katja Tuemmers
//The DisplayHighScore component contains references to two text objects to display name and score. It also has a level and spot variable
//which serve to identify itself. The DisplayHighScore checks for its own data by listening to the displayHighscoreSub event in the management system
//If its not the spot and level it ignores the data being passed
//This was done to make the highscore ranking as flexible as possible and ranking could be extendend for any number of Levels or spots in the ranking
//as it can exists indepedently from other objects and listens for itself for the needed information. When the HighScorePage script triggers the event in 
//the ManagementSystem all currently active DisplayHighScore objects update themselves through the thrown event, regardless how many there are or for many level 
//given that the ManagementSystem updates their spot and level. 
//Since only one Highscore Display page is active at a time, the currently not needed DisplayHighScore objects are inactive and dont update until their page is activated
public class DisplayHighScore : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int level; //what level the score ranking is for
    [SerializeField]
    private int spot;//spot on the ranking. First rank, second rank, etc.

    [SerializeField]
    private TMPro.TextMeshProUGUI name;
    [SerializeField]
    private TMPro.TextMeshProUGUI score;

    void Awake()
    {
        ManagementSystem.Instance.displayHighscoreSub += displayScore;
    }
    //Checks whether its beind adressed by the event, and if so updates itself with the new data.
    private void displayScore(string name, int score, int level, int spot)
    {
        if (this.spot == spot && this.level == level)
        {
            this.name.text = name;
            this.score.text = score.ToString();
        }

    }

}

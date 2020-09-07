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
    //private Button newHighscore;

    // Start is called before the first frame update
    void Awake()
    {
        ManagementSystem.displayHighscoreSub += displayScore;

    }

    private void displayScore(string name, int score, int level, int spot)
    {

    }

    // Update is called once per frame
    
}

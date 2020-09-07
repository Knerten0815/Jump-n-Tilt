using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DisplayHighScore : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int level;
    [SerializeField]
    private int spot;

    [SerializeField]
    private TMPro.TextMeshProUGUI name;
    [SerializeField]
    private TMPro.TextMeshProUGUI score;

    void Awake()
    {
        ManagementSystem.displayHighscoreSub += displayScore;
        Debug.Log("AAHHHHHH");
    }

    private void displayScore(string name, int score, int level, int spot)
    {
        if (this.spot == spot && this.level == level)
        {
            this.name.text = name;
            this.score.text = score.ToString();
            Debug.Log("score " + level + " spot " + spot);
        }

    }

}

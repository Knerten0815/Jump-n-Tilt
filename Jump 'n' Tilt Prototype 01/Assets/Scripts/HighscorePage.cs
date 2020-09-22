using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Katja Tuemmers
//When a highscore displaying page in a menu is started, it tells ManagementSystem to update the rankings
public class HighscorePage : MonoBehaviour
{
    [SerializeField]
    private int level;
    private void Start()
    {
        ManagementSystem.Instance.displayHighscoreOneLevel(level);
    }
}

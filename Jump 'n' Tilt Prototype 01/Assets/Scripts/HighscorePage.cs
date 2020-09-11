using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscorePage : MonoBehaviour
{
    [SerializeField]
    private int level;
    private void Start()
    {
        ManagementSystem.displayHighscoreOneLevel(level);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
/*
* Structure for the Saves. Can be serialized and turned into binary files. Attributes represent the saved data. Currently just a list
* of IDs of gathered Collectibles
*
* @Katja
*/
[System.Serializable]
public class Save
{

    [System.Serializable]
    public class ScorePair{
        public int score;
        public string name;

        public ScorePair(int score, string name)
        {
            this.name = name;
            this.score = score;
        }

    }
    public List<int> collectiblesGathered = new List<int>();
    public int currentLevel = 0;
    public ScorePair [][] scoreList = new ScorePair[3][];
    public int unlockedLevels = 0;
    public int currentScore;
}


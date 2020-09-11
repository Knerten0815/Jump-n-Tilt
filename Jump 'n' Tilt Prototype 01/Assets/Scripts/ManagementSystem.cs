using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
/*
*
* Class that overs Events to subscribe too. Can be accessed by other classes to trigger certain events that are subscribed too by all 
* needed parties. Also handles loading and saving game currently. Temporary, probably needs a better architecture and save options should 
* only be accessed by limited other classes and not everyone for example.
* 
*
*@Katja
*/
public class ManagementSystem : MonoBehaviour
{

    private static List<int> collectiblesGathered = new List<int>();
    private static int currentLevel = 0;
    private static Save.ScorePair[][] scoreList = new Save.ScorePair[3][];
    private static int unlockedLevels = 0;
    private static int currentScore;

    /*
    * Cheap and not perfect Singleton initialization. 
    *
    *
    *@Katja
    */
    private static ManagementSystem _instance;
    public static ManagementSystem Instance


    {
        get
        {
            if (_instance == null)
            {
                GameObject sSystem = new GameObject("ManagementSystem");
                sSystem.AddComponent<ManagementSystem>();
            }
            return _instance;
        }
    }


    /*
    *
    * Static function that when called adds the ID of the collectible to the currently gathered Collectibles
    *
    *@Katja
    */
    public static void AddCollectible(int n)
    {
        collectiblesGathered.Add(n);
    }

    /*
    * ScoreUp Event, can trigger the score going up but also things like Audio tracks for the pickUp
    *
    * @Katja
    */

    public delegate void scoreUp(int score);
    public static event scoreUp pickUpHit;
    /*
    *
    * Static function that when called triggers the event for scoreUp. 
    *
    * @Katja
    */
    public static void pickUp(int scoreValue)
    {
        currentScore += scoreValue;
        if (pickUpHit != null)
            pickUpHit(scoreValue);
    }


    public delegate void levelLoad(int unlockedLevels, int currentLevel);

    public static event levelLoad levelLoadMethod;
    /*
    *
    *   CollectibleLoad Event that currently is just there to notify rare what collectibles have already been Loaded could be expanded to other things
    *   that are loaded at the start
    *
    * @Katja
    */
    public delegate void pickupLoad(int collectibleID);
    public static event pickupLoad collectibleOnLoad;

    /*
    *
    *
    *
    * @Katja    
    */
    public delegate void displayHS(string name, int score, int level, int spot);
    public static event displayHS displayHighscoreSub;

    public static void displayHighscoreOneLevel(int level)
    {
        for (int i = 0; i < 5; i++)
        {
            if (displayHighscoreSub != null)
            {
                displayHighscoreSub(scoreList[level][i].name, scoreList[level][i].score, level, i);
                Debug.Log("are you displaying " + i);
            }
        }
    }


    /*
    *
    *   LoadGame is called before start and then rare pick ups are notified whether they were already gathered and have to be set inactive
    *   Does not work in htis prototype version currently:
    *   NullReferenceException: Object reference not set to an instance of an object
    *   ManagementSystem.Awake () (at Assets/Scripts/ManagementSystem.cs:100)
    *   No fucking clue why honestly. 
    * @Katja
    */

    public void Start()
    {

        LoadGame();
        // collectiblesGathered.Clear();
        //collectiblesGathered.Add(1);
        //collectiblesGathered.Add(2);
        //collectiblesGathered.Add(0);
        //unlockedLevels = 2;
        //currentLevel = 2;

        if (collectibleOnLoad != null)
        {
            foreach (int ID in collectiblesGathered)
            {
                collectibleOnLoad(ID);
            }
        }

        if (levelLoadMethod != null)
            levelLoadMethod(unlockedLevels, currentLevel);
    }



    public delegate void healthCurrent(int health);
    public static event healthCurrent healthPassOn;

    public static void updatePlayerHealth(int health)
    {
        Debug.Log("TEST hPuP");
        if (healthPassOn != null)
            healthPassOn(health);
    }

    public delegate void updateTimeSub(float time);
    public static event updateTimeSub timePassOn;
    public static void updateTime(float time)
    {
        if(timePassOn!=null)
            timePassOn(time);
    }

    public delegate void pickupHealth();
    public static event pickupHealth healthPickUpHit;
    public static void hUp()
    {
        //Debug.Log("TEST hUp");
        healthPickUpHit();
    }

    public delegate void collected();
    public static event collected collectedScroll;

    public static void collectedUpdate()
    {
        Debug.Log("TEST cUp");
        if (collectedScroll != null)
            collectedScroll();
    }


    public delegate void pickupTime();
    public static event pickupTime timePickUpHit;

    public static void tUp()
    {
        //Debug.Log("TEST tUp");
        timePickUpHit();
    }

    public static void endLevel()
    {
        SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }
    /*
    * CreateSaveGameObject creates an empty Save object and overrides it's collectiblesGathered attribute with the current
    * ManagementSystem version
    *
    *@Katja
    */
    private static Save CreateNewSaveGameObject()
    {
        Save save = new Save();
        collectiblesGathered = new List<int>();

        collectiblesGathered.Add(0);
        collectiblesGathered.Add(1);
        collectiblesGathered.Add(2);
        collectiblesGathered.Add(3);
        collectiblesGathered.Add(4);
        collectiblesGathered.Add(5);
        collectiblesGathered.Add(6);
        collectiblesGathered.Add(7);
        collectiblesGathered.Add(8);
        for (int i = 0; i < scoreList.Length; i++)
        {
            scoreList[i] = new Save.ScorePair[5];
            for (int j = 0; j < scoreList[i].Length; j++)
            {
                scoreList[i][j] = new Save.ScorePair(1000, "Player");
            }
        }
        save.collectiblesGathered = collectiblesGathered;
        save.currentLevel = 0;
        save.scoreList = scoreList;
        save.unlockedLevels = 2;
        unlockedLevels = save.unlockedLevels;
        currentLevel = 0;
        collectiblesGathered = save.collectiblesGathered;

        return save;

    }
    private static Save updateSaveGameObject()
    {
        Save save = new Save();
        save.currentScore = currentScore;
        save.collectiblesGathered = collectiblesGathered;
        save.currentLevel = currentLevel;
        save.scoreList = scoreList;
       // Debug.Log(save.scoreList[currentLevel - 1][0].name);
        save.unlockedLevels = unlockedLevels;
        return save;

    }

    public static bool newHighScore(string name, int spot)
    {
       
        Save.ScorePair newPair = new Save.ScorePair(currentScore, name);
        Save.ScorePair oldPair;
        for (int i = spot; i<scoreList[currentLevel].Length; i++)
        {
            Debug.Log("Old and new score");
            oldPair = scoreList[currentLevel][i];
            Debug.Log(oldPair.name + " und " + oldPair.score);
            Debug.Log(newPair.name + " und " + newPair.score);

            scoreList[currentLevel][i] = newPair;
            newPair = oldPair;
        }

        return true;
    }


    public static (int, int, int) checkForNewHighScore()
    {
        int spot = -1;
        for (int i = 4; i >= 0; i--)
        {
            if (currentScore > scoreList[currentLevel][i].score)
            {
                spot = i;
            }
            else
                break;
        }
        return (spot, currentLevel, currentScore);

    }

    public static void nextLevel()
    {
       
        currentLevel++;
        if (unlockedLevels < currentLevel)
        {
            unlockedLevels++;
        }
        if (currentLevel >= 3)
        {
            currentLevel = 0;
        }
   
        currentScore = 0;
        SaveGame();
        if (currentLevel == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevel + 1);
        }
    }
    /*
     * 
     * Creates binary data from Save object and saves it in file titles /gamesave.save
     * 
     * @Katja
     */
    private static void saveFile(Save save)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }
    public static void SaveGame()
    {

        Save save = updateSaveGameObject();
        saveFile(save);
    }

    private static void SaveNewGame()
    {
        Save save = CreateNewSaveGameObject();
        saveFile(save);
    }

 
    public static void loadLevel(int level)
    {
        Debug.Log("okay what s going on" + level);
        currentLevel = level;
        currentScore = 0;
        SaveGame();
        SceneManager.LoadScene(level+1);
    }

    public static void ResetGameSave()
    {
        collectiblesGathered = new List<int>();
        currentLevel = 0;
        //Highscore = new int[] { 0, 0, 0 };
        unlockedLevels = 2;
        currentScore = 0;
        Save save = CreateNewSaveGameObject();
        saveFile(save);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    /*
     * 
     * Checks first if save File exists. If it exists it translates the binary format back to a Save object instance which
     * is then saved in the management systems attribute of collectiblesGathered
     * 
     * @Katja
    */
    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            collectiblesGathered = save.collectiblesGathered;
            unlockedLevels = save.unlockedLevels;
            currentLevel = save.currentLevel;
            scoreList = save.scoreList;
            Debug.Log("Does it load");
        }
        else {
            SaveNewGame();
        }
    }
}

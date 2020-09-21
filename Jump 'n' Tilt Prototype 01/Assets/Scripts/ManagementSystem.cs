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
/*
/*
*
* Author: Katja Tuemmers 
* Loads and Creates the Save Game. It lives through the complete runtime of the game.
* Class that offers Events to subscribe too to pass information between objects or information from the Save file on. 
* Can be accessed by other classes to trigger certain events that are subscribed too by other objects like when a pickUp is picked up
* Handles the progress and switches of the game between different scenes
* Has a load screen that can be temporarly activated when a scene change is initiated
* 
*
*/
public class ManagementSystem : MonoBehaviour
{

    private List<int> collectiblesGathered = new List<int>();
    private int currentLevel = 0;
    private Save.ScorePair[][] scoreList = new Save.ScorePair[3][];
    private int unlockedLevels = 0;

    private int currentScore;

    public GameObject loadscreen;
    public static ManagementSystem Instance { get; private set; }
    
    
    /*
     * 
    * Quick Singleton initialization and subscription to sceneLoaded. ManagementSystem is not destroyed between scenes!
    *
    *
    */

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += startLevel; //Subscribes to sceneLoaded to pass on information via startLevel once a scene is loaded
    }
    /*
    * When the ManagementSystem is first enabled in the first scene it loads the game
    *
    */
    private void OnEnable()
    {
        LoadGame();
    }


    /*
    * Events and public functions that trigger events
    * 
    */



     //event passes on which levels are currently unlocked and which level has been played last or is currently played
    public delegate void levelLoad(int unlockedLevels, int currentLevel);
    public event levelLoad levelLoadMethod;
    
    /*
    *
    *   CollectibleLoad Event that currently is just there to notify rare what collectibles have already been Loaded could be expanded to other things
    *   that are loaded at the start
    *
    * 
    */
    public delegate void pickupLoad(int collectibleID);
    public event pickupLoad collectibleOnLoad;

    /*
    * Event that tells a DisplayHighscore objects with the same level and spot to update itself with the passed on information
    * is currently done for five rankings at a time
    *  
    */
    public delegate void displayHS(string name, int score, int level, int spot);
    public event displayHS displayHighscoreSub;

    public void displayHighscoreOneLevel(int level)
    {
        for (int i = 0; i < 5; i++)
        {
            if (displayHighscoreSub != null)
            {
                displayHighscoreSub(scoreList[level][i].name, scoreList[level][i].score, level, i);
            }
        }
    }

    //When the playercharacter calls updatePlayerHealth and passes on its current health the information is passed onto all subscribers
    //like the scoring system which displays the current health
    public delegate void healthCurrent(int health);
    public event healthCurrent healthPassOn;
    
    public void updatePlayerHealth(int health)
    {
        if (healthPassOn != null)
        {
            healthPassOn(health);
        }
    }

    //When the playercharacter calls updateTime and passes on its current time that it can slow down and the information is passed onto all subscribers
    //like the scoring system which displays the current time available to be slowed down
    public delegate void updateTimeSub(float time);
    public event updateTimeSub timePassOn;
    public void updateTime(float time)
    {
        if(timePassOn!=null)
            timePassOn(time);
    }
      /*
    *
    * Function that when called adds the ID of the collectible to the currently gathered Collectibles, also trigges the collectibleOnLoad event
    * that would inform listeners that this specific pickUp has been collected. In this case its the buttons in menu that now grant access to display
    * the newly found collectible and its information and images.
    *
    *
    */
    public void addCollectible(int ID)
    {
        collectiblesGathered.Add(ID);
        if (collectibleOnLoad != null)
            collectibleOnLoad(ID);
    }

    /*
    * ScoreUp Event notifies listeners when the score increases
    *
    */

    public delegate void scoreUp(int score);
    public event scoreUp pickUpHit;

    /*
    *
    * Function that when called triggers the event for scoreUp and passes on which value of points should be added to the score, is usually
    * called by the pickUpDescriptor once the Player triggers the hit function
    *
    * 
    */
    public void pickUp(int scoreValue)
    {
        currentScore += scoreValue;
        if (pickUpHit != null)
            pickUpHit(scoreValue);
    }

    //A health pick up has been picked up 
    public delegate void pickupHealth();
    public event pickupHealth healthPickUpHit;
    public void hUp()
    {
        if (healthPickUpHit != null)
        {
            healthPickUpHit();
        }
    }

    //A collectible has been picked up
    public delegate void collected();
    public event collected collectedScroll;

    public void collectedUpdate()
    {
        if (collectedScroll != null)
            collectedScroll();
    }

    //A time pickup has been picked up
    public delegate void pickupTime();
    public event pickupTime timePickUpHit;

    public void tUp()
    {
        if (timePickUpHit != null)
        {
            timePickUpHit();
        }
    }

    //Inserts a new highscore that has been reached in the ranking list
    public bool newHighScore(string name, int spot)
    {
       
        Save.ScorePair newPair = new Save.ScorePair(currentScore, name);
        Save.ScorePair oldPair;
        for (int i = spot; i<scoreList[currentLevel].Length; i++)
        {
            oldPair = scoreList[currentLevel][i];
            scoreList[currentLevel][i] = newPair;
            newPair = oldPair;
        }

        return true;
    }

    //checks if a new highscore has been achieved
    public (int, int, int) checkForNewHighScore()
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



    /*
     * LEVEL AND SCENE LOADING FUNCTIONS
     * 
     */




    /*
    *
    *  startLevel is called whenever a new scene is loaded by subscribing to Management.sceneLoaded
    *  it resets the time scale to unfreeze a level
    *  it then loops through all the collectible that have been collected by the player and throws the collectibleOnLoad event
    *  the collectibles in the scene listen to that and if they were collected before modify themselves to a normal coin pick up
    *  the levelLoadMethod informs all its subscribers which levels have been unlocked and which level is currently playing or has been played last    
    */

    private void startLevel(Scene scene, LoadSceneMode mode)
    {


        Time.timeScale = 1f;
        if (collectibleOnLoad != null)
        {
            foreach (int ID in collectiblesGathered)
            {
                if (collectibleOnLoad != null)
                    collectibleOnLoad(ID);
            }
        }

        if (levelLoadMethod != null)
            levelLoadMethod(unlockedLevels, currentLevel);
        loadscreen.SetActive(false);
    }

    //ends a level and opens the highscore scene
    public void endLevel()
    {
        //loadscreen.SetActive(true);
        SceneManager.LoadScene(4);
    }
    //restarts the currently played level
    public void restartLevel()
    {
        currentScore = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevel + 1);
    }
    //From the highscore scene the next level is loaded. If a new level has been unlocked the unlockedLevels are increased
    //the currentLevel is increased unless the maximum is reached, if so its set to 0 and the credits scene is loaded
    //the game is saved
    public void nextLevel()
    {
        loadscreen.SetActive(true);

        currentLevel++;
        if (currentLevel >= 3)
        {
            currentLevel = 0;
        }
        if (unlockedLevels < currentLevel && unlockedLevels<3)
        {
            unlockedLevels++;
        }
     
   
        currentScore = 0;
        SaveGame();
        if (currentLevel == 0)
        {
            //loadscreen.SetActive(true);
            UnityEngine.SceneManagement.SceneManager.LoadScene(7);
            
        }
        else
        {
            //loadscreen.SetActive(true);
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevel + 1);
        }
    }
    //specific level is loaded
    public void loadLevel(int level)
    {
        loadscreen.SetActive(true);
        currentLevel = level;
        currentScore = 0;
        SaveGame();
        SceneManager.LoadScene(level + 1);
    }


    /*
    *
    * Save File manipulation and creation
    * 
    */



    /*
     * 
     * Creates binary data from Save object and saves it in file titles /gamesave.save
     * 
     */
    private void saveFile(Save save)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }

    /*
    * CreateSaveGameObject creates a fresh Save object and also overrides the ManagementSystem versions of collectiblesGathered and other variables describing the save state
    * with the new fresh values
    * place holder highscore rankings are created
    * 
    *
    */
    private Save CreateNewSaveGameObject()
    {
        Save save = new Save();
        collectiblesGathered = new List<int>();
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
        save.unlockedLevels = 0;
        unlockedLevels = save.unlockedLevels;
        currentLevel = 0;

        return save;

    }
    //creates a save oject with the current data
    private Save updateSaveGameObject()
    {
        Save save = new Save();
        save.currentScore = currentScore;
        save.collectiblesGathered = collectiblesGathered;
        save.currentLevel = currentLevel;
        save.scoreList = scoreList;
        save.unlockedLevels = unlockedLevels;
        return save;

    }
    //saves the same
    public void SaveGame()
    {

        Save save = updateSaveGameObject();
        saveFile(save);
    }
    //creates a fresh save game
    private void SaveNewGame()
    {
        Save save = CreateNewSaveGameObject();
        saveFile(save);
    }


    //game save is reset and level 1 is started
    public void ResetGameSave()
    {
        loadscreen.SetActive(true);
        SaveNewGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    /*
     * 
     * Checks first if save File exists. If it exists it translates the binary format back to a Save object instance which
     * is then passed on to respective the ManagementSystems attributes 
     * 
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
        }
        else {
            SaveNewGame();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
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
    private static int[] Highscore = { 0, 0, 0 };
    private static int unlockedLevels = 0;

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
        pickUpHit(scoreValue);
    }
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
    *   LoadGame is called before start and then rare pick ups are notified whether they were already gathered and have to be set inactive
    *   Does not work in htis prototype version currently:
    *   NullReferenceException: Object reference not set to an instance of an object
    *   ManagementSystem.Awake () (at Assets/Scripts/ManagementSystem.cs:100)
    *   No fucking clue why honestly. 
    * @Katja
    */

    public void Awake()
    {

        LoadGame();
       
        foreach (int ID in collectiblesGathered)
        {
            //Debug.Log(ID);
            collectibleOnLoad(ID);

        }



    }

    public delegate void pickupHealth();
    public static event pickupHealth healthPickUpHit;

    public static void hUp()
    {
        //Debug.Log("TEST hUp");
        healthPickUpHit();
    }


    public delegate void pickupTime();
    public static event pickupTime timePickUpHit;

    public static void tUp()
    {
        //Debug.Log("TEST tUp");
        timePickUpHit();
    }


    /*
    * CreateSaveGameObject creates an empty Save object and overrides it's collectiblesGathered attribute with the current
    * ManagementSystem version
    *
    *@Katja
    */
    private static Save CreateSaveGameObject()
    {
        Save save = new Save();
        save.collectiblesGathered = collectiblesGathered;
        save.currentLevel = 0;
        save.Highscore = Highscore;
        save.unlockedLevels = 0;
        return save;

    }

    /*
     * 
     * Creates binary data from Save object and saves it in file titles /gamesave.save
     * 
     * @Katja
     */
    public static void SaveGame()
    {
        
        Save save = CreateSaveGameObject();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
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
            Highscore = save.Highscore;
            unlockedLevels = save.unlockedLevels;
            currentLevel = save.currentLevel;
            Debug.Log("Does it load");
        }
        else { 
            SaveGame();
            }

    }
}

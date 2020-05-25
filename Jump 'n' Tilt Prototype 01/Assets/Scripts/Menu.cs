using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameActions;
using UnityEngine.UI;

// Author: Michelle Limbach
//This class manages the Input in the Menu scene
public class Menu : MonoBehaviour
{
 
    //This method loads a scene (later: a level) by a given name
    //Used by Button_StartGame
    public void startGame (string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }

    //This method closes the whole Application
    //Used by Button_ExitGame
    public void exitGame()
    {
        Application.Quit();
    }

  
}

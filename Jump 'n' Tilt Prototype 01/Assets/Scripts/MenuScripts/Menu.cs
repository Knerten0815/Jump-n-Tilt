using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using GameActions;
using UnityEngine.UI;
using TMPro;

// Author: Michelle Limbach
//This class manages the Input of the start and pause Menu
namespace menuHandling
{
    public class Menu : MonoBehaviour
    {
        public GameObject firstSelected; //First selected game object of the first menu page
        public GameObject currentMenuPage; 
        EventSystem m_EventSystem; //Current eventsystem
        public GameObject inputvis; //Game object of the input visualization


        //Shows the first Menu page
        public void OnEnable()
        {
            m_EventSystem = EventSystem.current; //Get current eventsystem
            m_EventSystem.SetSelectedGameObject(firstSelected); //Set first game object of menu page as selected game object of the event system
            firstSelected.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold; //Change the font style of first game object
            firstSelected.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f); // Change color of first game object

        }

        //Used to return to menu
        public void startGame()
        {
            SceneManager.LoadScene(0);
        }

        //Deletes all progress and restartes the whole game save
        public void restartGame()
        {
            ManagementSystem.Instance.ResetGameSave();
        }
        //This method closes the whole Application
        //Used by Button_ExitGame
        public void exitGame()
        {
            Application.Quit();
        }

        //This method switches between fullscreen and window mode
        //Used by the fullscreen slider
        public void SetFullscreen(Slider slider)
        {
            if(slider.value == 0)
            {
                Screen.fullScreen = false;
            }
            else
            {
                Screen.fullScreen = true;
            }
        }
        //This method displays the InputVisualization
        //Used by the Input slider
        public void DisplayInputVisualization(Slider slider)
        {
            if (slider.value == 0)
            {
                inputvis.SetActive(false);
            }
            else
            {
                inputvis.SetActive(true);
            }
        }


    }
}
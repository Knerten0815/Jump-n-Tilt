using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using GameActions;
using UnityEngine.UI;
using TMPro;

// Author: Michelle Limbach
//This class manages the Input in the Menu
namespace menuHandling
{
    public class Menu : MonoBehaviour
    {
        public GameObject firstSelected;
        public GameObject currentMenuPage;
        EventSystem m_EventSystem;
        public GameObject inputvis;



        //public GameObject schriftrolle;

        public void OnEnable()
        {
            m_EventSystem = EventSystem.current;
            m_EventSystem.SetSelectedGameObject(firstSelected);
            firstSelected.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
            firstSelected.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);

        }
        //This method loads a scene (later: a level) by a given name
        //Used by Button_StartGame
        public void startGame(string scene_name)
        {
            SceneManager.LoadScene(scene_name);
        }

        public void restartGame()
        {
            ManagementSystem.ResetGameSave();
        }
        //This method closes the whole Application
        //Used by Button_ExitGame
        public void exitGame()
        {
            Application.Quit();
        }

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
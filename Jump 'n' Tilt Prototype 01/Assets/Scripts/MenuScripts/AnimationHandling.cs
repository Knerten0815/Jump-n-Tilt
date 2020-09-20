using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameActions;

//Author: Michelle Limbach
//This script handels the Animation for opening and closing the menu and also some basic functionalities of the menu

namespace menuHandling
{
    public class AnimationHandling : MonoBehaviour
    {
        private GameObject menuBackground; //Scroll image
        private GameObject firstMenuPage; 
        
        private Animator animator; //animator of the menu
        private bool showPauseMenu = false; //variable to check if the pause menu is currently opened

        public void Start()
        {
            PlayerInput.onMenuDown += pauseGame; //subscribe to onMenuDown event
            menuBackground = GameObject.Find("Schriftrolle");
            firstMenuPage = GameObject.Find("Main Menu");
            animator = menuBackground.GetComponent<Animator>(); 

            //Checks if you are currently in one of the following scenes: StartMenu, HighScore, Credits
            //If yes then show start menu
            if (SceneManager.GetActiveScene().name == "StartMenu" || SceneManager.GetActiveScene().name == "HighScore" || SceneManager.GetActiveScene().name == "Credits")
            {
                firstMenuPage.SetActive(false);
                animator.SetTrigger("OpenScroll");
                StartCoroutine(ShowMenu());
            }
            else //if not then pause menu
            {
                menuBackground.SetActive(false);
                firstMenuPage.SetActive(false);
            }
        }
        private void OnDisable()
        {
            PlayerInput.onMenuDown -= pauseGame;
        }

        //Coroutine to wait with showing the content of the menu until the background scroll is completely opened
        IEnumerator ShowMenu()
        {
            yield return new WaitForSecondsRealtime(0.5f);
  
            firstMenuPage.SetActive(true);
        }
        //Coroutine to wait with disabling the background until the scroll is completely closed. After that the player gets back the controls
        IEnumerator DisableMenu()
        {
            yield return new WaitForSecondsRealtime(1f);
      
            menuBackground.SetActive(false);
            Time.timeScale = 1f;

        }

        //Function to close the pause menu
        public void continueGame()
        {
            firstMenuPage.SetActive(false);
            animator.SetTrigger("CloseScroll");
            StartCoroutine(DisableMenu());
            showPauseMenu = false;
        }
        //Function to open the pause menu
        private void pauseGame()
        {
          //If the pause menu is currently not opended, then open it
            if (!showPauseMenu)
            {
              
                DialogueManager.Instance.EndDialogue(); //If there is currently a dialogue displayed then close it
                Time.timeScale = 0f; //Pause gameplay
                menuBackground.SetActive(true);
                animator.SetTrigger("OpenScroll");
                StartCoroutine(ShowMenu());
                showPauseMenu = true;
    
            }
            else //If the pause menu is currently opend then close it
            {
             
                foreach (Transform child in transform)
                {
                    if (child.gameObject.activeSelf && child.transform.name != "Schriftrolle") {
                        child.gameObject.SetActive(false);
                        animator.SetTrigger("CloseScroll");
                        StartCoroutine(DisableMenu());
                        showPauseMenu = false;
                    }
                }
            }

        }
    }
}
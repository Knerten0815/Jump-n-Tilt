using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameActions;

//Author: Michelle Limbach
//This script handels the Animation for opening and closing the menu

namespace menuHandling
{
    public class AnimationHandling : MonoBehaviour
    {
        private GameObject menuBackground;
        private GameObject firstMenuPage;
        
        private Animator animator;
        private bool showPauseMenu = false;

        public void Start()
        {
            PlayerInput.onMenuDown += pauseGame;
            menuBackground = GameObject.Find("Schriftrolle");
            firstMenuPage = GameObject.Find("Main Menu");
            Debug.Log(showPauseMenu);
            animator = menuBackground.GetComponent<Animator>();


            if (SceneManager.GetActiveScene().name == "NewMenu" || SceneManager.GetActiveScene().name == "StartMenu" || SceneManager.GetActiveScene().name == "HighScore" || SceneManager.GetActiveScene().name == "Credits")
            {
                firstMenuPage.SetActive(false);
                animator.SetTrigger("OpenScroll");
                StartCoroutine(ShowMenu());
            }
            else
            {
                menuBackground.SetActive(false);
                firstMenuPage.SetActive(false);
            }
        }
        private void OnDisable()
        {
            PlayerInput.onMenuDown -= pauseGame;
        }

        IEnumerator ShowMenu()
        {
            yield return new WaitForSecondsRealtime(0.5f);
  
            firstMenuPage.SetActive(true);
        }

        IEnumerator DisableMenu()
        {
            yield return new WaitForSecondsRealtime(1f);
      
            menuBackground.SetActive(false);

        }


        public void continueGame()
        {
            firstMenuPage.SetActive(false);
            animator.SetTrigger("CloseScroll");
            StartCoroutine(DisableMenu());
            Time.timeScale = 1f;
            showPauseMenu = false;
        }

        private void pauseGame()
        {
         
            if (!showPauseMenu)
            {
              
                DialogueManager.Instance.EndDialogue();
                Time.timeScale = 0f;
                menuBackground.SetActive(true);
                animator.SetTrigger("OpenScroll");
                StartCoroutine(ShowMenu());
                showPauseMenu = true;
    
            }
            else
            {
             
                foreach (Transform child in transform)
                {
                    if (child.gameObject.activeSelf && child.transform.name != "Schriftrolle") {
                        child.gameObject.SetActive(false);
                        animator.SetTrigger("CloseScroll");
                        StartCoroutine(DisableMenu());
                        Time.timeScale = 1f;
                        showPauseMenu = false;
                    }
                }
            }

        }
    }
}
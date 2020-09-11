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


            if (SceneManager.GetActiveScene().name == "NewMenu" || SceneManager.GetActiveScene().name == "KatjaMenue2" || SceneManager.GetActiveScene().name == "HighScore")
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
            Debug.Log("did you wait?");
            firstMenuPage.SetActive(true);
        }

        IEnumerator DisableMenu()
        {
            yield return new WaitForSecondsRealtime(1f);
            Debug.Log("did you wait before disabling?");
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
            Debug.Log("is pauseGame happening");
            if (!showPauseMenu)
            {
                Debug.Log("Are you opening?");

                DialogueManager.Instance.EndDialogue();
                Time.timeScale = 0f;
                menuBackground.SetActive(true);
                animator.SetTrigger("OpenScroll");
                StartCoroutine(ShowMenu());
                showPauseMenu = true;
                Debug.Log("Are you finished opening?");
            }
            else
            {
                Debug.Log("Are you closing?");
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
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

        public void Start()
        {
            PlayerInput.onMenuDown += pauseGame;
            menuBackground = GameObject.Find("Schriftrolle");
            firstMenuPage = GameObject.Find("Main Menu");
            animator = menuBackground.GetComponent<Animator>();


            if (SceneManager.GetActiveScene().name == "NewMenu" || SceneManager.GetActiveScene().name == "KatjaMenue2")
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
        }

        private void pauseGame()
        {
            DialogueManager.Instance.EndDialogue();
            Time.timeScale = 0f;
            menuBackground.SetActive(true);
            animator.SetTrigger("OpenScroll");
            StartCoroutine(ShowMenu());

        }
    }
}
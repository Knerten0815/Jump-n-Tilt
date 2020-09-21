using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//Author: Katja Tuemmers
//Manipulates Start and Pause menu objects to reflect current save game
public class DisplayCollectible : MonoBehaviour
{

    public CollectibleCard collectible; //Only CollectibleCard loaded at a time
    
    public TextMeshProUGUI title; //Reference to the text object for the title of the Collectible Card the user wishes to view and displayed in DisplayScreen
    public TextMeshProUGUI description;//Reference to the text object for the description of the Collectible Card the user wishes to view and displayed in DisplayScreen
    public RawImage image;//Reference to the image for the the Collectible Card the user wishes to view and displayed in DisplayScreen
    public Button[] buttons; //Buttons that link to the DisplayScreen and activate viewing a collectible
    public Button[] levelButtons;//Buttons for Level selection
    public Button continueButton;//Button for restarting a level played last. In start menu continue button, in pause menu restart level button

    //Subscribing to the ManagementSystem to recieve informations about the save game 
    void Awake()
    {
       ManagementSystem.Instance.collectibleOnLoad += overwrite;
       ManagementSystem.Instance.levelLoadMethod += levelInteractable;

    }
    //Assigns functions to the levelButtons and continueButton that would call the functions to change the level in the ManagementSystem
    public void levelInteractable(int unlockedLevels, int currentLevel)
        /*
        * Depending on how many levels were unlocked the buttons in the Select Level menu are set to interactable and a listener is added
        * so the setLoadLevel is executed on click. The index of the loop is used as the desired level to be loaded
        * 
        */
    {   if (levelButtons.Length != 0)
        {
            for (int i = 0; i <= unlockedLevels; i++)
            {
                int temp = i;
                levelButtons[i].interactable = true;
                levelButtons[i].onClick.AddListener(() => setLoadLevel(temp));
            }
        }
        continueButton.onClick.AddListener(() => setLoadLevel(currentLevel));
    }
    //Loads the desired level by calling method in the ManagementSystem 
    private void setLoadLevel(int level)  
    {
        ManagementSystem.Instance.loadLevel(level);
    }
    //Changes a Collectible Menu button text to its name from "???" and makes the button interactable
    public void overwrite(int ID)
    {
        buttons[ID].GetComponentInChildren<TextMeshProUGUI>().text = buttons[ID].name;
        buttons[ID].interactable = true;
    }
    //Updates the DisplayScreen texts and images to the current CollectibleCard
    public void update()
    {
        title.text = collectible.title;
        description.text = collectible.description;
        image.texture = collectible.artwork.texture;
    }
    //Unsubscribes
    private void OnDisable()
    {
        ManagementSystem.Instance.collectibleOnLoad -= overwrite;
        ManagementSystem.Instance.levelLoadMethod -= levelInteractable;
    }
}

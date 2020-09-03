using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using TMPro;

public class DisplayCollectible : MonoBehaviour
{

    public CollectibleCard collectible;
    
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Button[] buttons;
    //public string[] collectibleName;
    public Button[] levelButtons;
    public Button continueButton;
    public RawImage image;


    // Start is called before the first frame update
    void Awake()
    {
       ManagementSystem.collectibleOnLoad += overwrite;
       ManagementSystem.levelLoadMethod += levelInteractable;

    }
    public void levelInteractable(int unlockedLevels, int currentLevel)
    {   if (levelButtons.Length != 0)
        {
            for (int i = 0; i <= unlockedLevels; i++)
            {
                levelButtons[i].interactable = true;
            }
        }
        continueButton.onClick.AddListener(() => ButtonClicked(currentLevel));
    }
    public void ButtonClicked(int currentLevel)
    {
        Debug.Log("AHHHH" + currentLevel);
    }
    public void overwrite(int ID)
    {
        buttons[ID].GetComponentInChildren<TextMeshProUGUI>().text = buttons[ID].name;
        buttons[ID].interactable = true;
    }
    public void update()
    {
        title.text = collectible.title;
        description.text = collectible.description;
        image.texture = collectible.artwork.texture;
    }
}

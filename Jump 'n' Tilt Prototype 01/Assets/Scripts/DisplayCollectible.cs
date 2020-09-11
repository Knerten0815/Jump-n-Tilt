using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
                Debug.Log("whats on level button " + currentLevel);

                levelButtons[i].interactable = true;
                levelButtons[i].onClick.AddListener(() => setLoadLevel(i));
            }
        }
        Debug.Log("whats on current button+ " + currentLevel);

        continueButton.onClick.AddListener(() => setLoadLevel(currentLevel));
    }
    public void setLoadLevel(int level)  
    {
        Debug.Log("what level when clicked + " + level);
        ManagementSystem.loadLevel(level);
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
    private void OnDisable()
    {
        ManagementSystem.collectibleOnLoad -= overwrite;
        ManagementSystem.levelLoadMethod -= levelInteractable;
    }
}

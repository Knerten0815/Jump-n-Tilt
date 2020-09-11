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
       ManagementSystem.Instance.collectibleOnLoad += overwrite;
       ManagementSystem.Instance.levelLoadMethod += levelInteractable;

    }
    public void levelInteractable(int unlockedLevels, int currentLevel)
    {   if (levelButtons.Length != 0)
        {
            Debug.Log("what levels are unlocked " + unlockedLevels);
            for (int i = 0; i <= unlockedLevels; i++)
            {
                Debug.Log("whats on level button " + i);
                int temp = i;
                levelButtons[i].interactable = true;
                levelButtons[i].onClick.AddListener(() => setLoadLevel(temp));
            }
        }
        Debug.Log("whats on current button+ " + currentLevel);

        continueButton.onClick.AddListener(() => setLoadLevel(currentLevel));
    }
    private void setLoadLevel(int level)  
    {
        Debug.Log("what level when clicked + " + level);
        ManagementSystem.Instance.loadLevel(level);
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
        ManagementSystem.Instance.collectibleOnLoad -= overwrite;
        ManagementSystem.Instance.levelLoadMethod -= levelInteractable;
    }
}

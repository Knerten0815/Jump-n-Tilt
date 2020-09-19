using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

//Author: Michelle Limbach
//This class is responsible for the button hovering, so that the user gets visual feedback which button is currently selected
public class MenuButtonHover : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler 
{


    EventSystem m_EventSystem; //current eventsystem
    private void Start()
    {
        m_EventSystem = EventSystem.current; //get current eventsystem

    }

    //Function gets called when the mouse pointer hovers over a button.
    //Then the color of the button gets changed and the hovered button is set to selected game object of the current event system
    public void OnPointerEnter(PointerEventData eventData)
    {
       

        if (this.GetComponent<Button>().interactable) //check if this button is interactable
        {
            this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
            this.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);
            m_EventSystem.SetSelectedGameObject(gameObject);
        }
    }

    //Function gets called when the button is selected via controller or keyboard input
    //Then the color of the button gets changed and the hovered button is set to selected game object of the current event system
    public void OnSelect(BaseEventData eventData)
    {
        if (this.GetComponent<Button>().interactable) //check if this button is interactable
        {
            this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
            this.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        }
    }
    //Function gets called when the button is deselected via controller or keyboard input
    //Then the color of the button gets changed and the hovered button is set to selected game object of the current event system
    public void OnDeselect(BaseEventData eventData)
    {
        if (this.GetComponent<Button>().interactable) //check if this button is interactable
        {
            this.GetComponentInChildren<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
            this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            this.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
    }
    //Function to style buttons if they are disabled
    public void OnDisable()
    {
        if (this.GetComponent<Button>().interactable)
        {
            this.GetComponentInChildren<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
            this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            this.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        }
    }
    //Function to style buttons if they are enabled
    public void OnEnable()
    {
        
        if(this.GetComponent<Button>().interactable == false)
        {
            this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
            this.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.3113208f, 0.3113208f, 0.3113208f);
      
        }
    }

}
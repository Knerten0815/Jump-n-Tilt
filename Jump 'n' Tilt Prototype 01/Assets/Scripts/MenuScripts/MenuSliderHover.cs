using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Author: Michelle Limbach
//This class is responsible for the slider hovering, so that the user gets visual feedback which slider is currently selected
public class MenuSliderHover : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    EventSystem m_EventSystem; //current eventsystem
    private void Start()
    {
        m_EventSystem = EventSystem.current; //get current eventsystem

    }
    //Function gets called when the mouse pointer hovers over a slider.
    //Then the color of the slider gets changed and the hovered slider is set to selected game object of the current event system
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        this.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        m_EventSystem.SetSelectedGameObject(gameObject);
    }
    //Function gets called when the slider is selected via controller or keyboard input
    //Then the color of the slider gets changed and the hovered slider is set to selected game object of the current event system
    public void OnSelect(BaseEventData eventData)
    {
        this.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        this.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
    }
    //Function gets called when the slider is deselected via controller or keyboard input
    //Then the color of the slider gets changed and the hovered slider is set to selected game object of the current event system
    public void OnDeselect(BaseEventData eventData)
    {
        this.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.1981132f, 0.1981132f, 0.1981132f);
        this.transform.GetChild(2).GetComponentInChildren<Image>().color = Color.black;
    }
    //Function to style slider if they are disabled
    public void OnDisable()
    {
        this.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.1981132f, 0.1981132f, 0.1981132f);
        this.transform.GetChild(2).GetComponentInChildren<Image>().color = Color.black;
    }
}

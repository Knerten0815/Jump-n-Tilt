using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//Author: Michelle Limbach
//This script handels the mouse hover of the menu
public class MenuButtonHover : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler //IPointerExitHandler 
{
    EventSystem m_EventSystem;
    private void Start()
    {
        m_EventSystem = EventSystem.current;
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        /*m_EventSystem.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
        m_EventSystem.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        m_EventSystem.currentSelectedGameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;*/

        this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
        this.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        m_EventSystem.SetSelectedGameObject(gameObject);
    }

    /*public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponentInChildren<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
        this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        this.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }*/

    public void OnSelect(BaseEventData eventData)
    {
        this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Underline | FontStyles.Bold;
        this.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0.6470f, 0.0627f, 0.0627f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        this.GetComponentInChildren<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
        this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        this.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }
    public void OnDisable()
    {
        this.GetComponentInChildren<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
        this.GetComponentInChildren<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        this.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }
}
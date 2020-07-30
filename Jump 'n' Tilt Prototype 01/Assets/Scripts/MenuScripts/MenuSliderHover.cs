using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuSliderHover : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
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

        //this.transform.GetChild(0).GetComponent<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        this.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        this.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        m_EventSystem.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        //this.transform.GetChild(0).GetComponent<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        this.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
        this.transform.GetChild(2).GetComponentInChildren<Image>().color = new Color(0.6470f, 0.0627f, 0.0627f);
    }
    public void OnDeselect(BaseEventData eventData)
    {

        // this.transform.GetChild(0).GetComponent<Image>().color = new Color(0.490566f, 0.490566f, 0.490566f);
        this.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.1981132f, 0.1981132f, 0.1981132f);
        this.transform.GetChild(2).GetComponentInChildren<Image>().color = Color.black;
    }
    public void OnDisable()
    {

        //this.transform.GetChild(0).GetComponent<Image>().color = new Color(0.490566f, 0.490566f, 0.490566f);
        this.transform.GetChild(1).GetComponentInChildren<Image>().color = new Color(0.1981132f, 0.1981132f, 0.1981132f);
        this.transform.GetChild(2).GetComponentInChildren<Image>().color = Color.black;
    }
}

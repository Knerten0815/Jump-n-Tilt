using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TESTINPUT : MonoBehaviour
{
    [SerializeField]
    private Button[] nope;
    [SerializeField]
    private Button btn;
    [SerializeField]
    private TMP_InputField input;

    void Start()
    {
        foreach (Button b in nope)
        {
            b.onClick.AddListener(() => ButtonClicked(b.name));
            Debug.Log(b.name);
        }
        btn.onClick.AddListener(() => ButtonClicked(btn.name));
        Debug.Log(btn.name);

        // Button btn = nope1.GetComponent<Button>();
        //    btn.onClick.AddListener(() => ButtonClicked(btn.GetComponent<Text>().text));
    }
    void ButtonClicked(string name)
    {
        Debug.Log("HALLO A IS BEING CLICKED MAN");
        input.text = input.text + name;
    }
}
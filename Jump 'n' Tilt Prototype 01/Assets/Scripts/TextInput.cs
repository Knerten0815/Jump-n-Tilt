using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextInput : MonoBehaviour
{
    [SerializeField]
    private Button[] btn;
    [SerializeField]
    private TMP_InputField input;
    [SerializeField]
    private Button backSpace;

    void Start()
    {
        foreach (Button b in btn)
        {
            b.onClick.AddListener(() => ButtonClicked(b.name));
        }

        // Button btn = nope1.GetComponent<Button>();
        backSpace.onClick.AddListener(() => backspacePressed());
    }
    void ButtonClicked(string name)
    {
        if (input.text.Length<6)
            input.text = input.text + name;
    }
    void backspacePressed()
    {
        if (input.text.Length > 0)
            input.text.Remove(input.text.Length - 1);
    }
    public string getInputText()
    {
        return input.text;
    }
}
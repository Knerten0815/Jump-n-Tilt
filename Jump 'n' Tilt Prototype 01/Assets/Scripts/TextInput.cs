using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Author: Katja Tuemmers
//Creates an in-game keyboard with arrays of buttons and input field
public class TextInput : MonoBehaviour
{
    [SerializeField]
    private Button[] btn; //array of buttons for letters
    [SerializeField]
    private TMP_InputField input; //input field
    [SerializeField]
    private Button backSpace; //Backspace button
    
    void Start()
    {
        //A listener is added to each button in the array. OnClick the function ButtonClicked() will be executed with the paramater defined 
        //for each button. In this its the name of the button. To create a keyboard the buttons just have to be named like the letter 
        //they represent on the keyboard
        foreach (Button b in btn)
        {
            b.onClick.AddListener(() => ButtonClicked(b.name));
        }
        //Backspace button gets a seperate listener and function when pressed
        backSpace.onClick.AddListener(() => backspacePressed());
    }
    //If the limit of six characters isn't exceeded the name of the button is added to the text in the input field
    void ButtonClicked(string name)
    {
        if (input.text.Length<6)
            input.text = input.text + name;
    }
    //When the backspace button is pressed the last character in the input field text is removed. 
    void backspacePressed()
    {
        if (input.text.Length > 0)
        {
            string sub = input.text.Remove(input.text.Length - 1);
            input.text = sub;
        }
    }
    public string getInputText()
    {
        return input.text;
    }
}
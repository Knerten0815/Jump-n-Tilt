using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;
using UnityEngine.UI;

// Author: Michelle Limbach
// This class visualizes the Player Input and can be used for debugging or for the Tutorial
// The events of this class are initalized in PlayerInput.cs
public class InputVisualization : MonoBehaviour
{

    private GameObject Button_Left;
    private GameObject Button_Right;
    private GameObject Button_A;
    private GameObject Button_B;
    private GameObject Button_X;
    private GameObject Button_Y;
    private GameObject Button_L;
    private GameObject Button_R;
 

    private Color highlighted = new Color(1f, 1f, 1f); //Color for the highlighted Buttons
    private Color dark = new Color(0.79f, 0.79f, 0.79f); //Color if the Buttons are not pressed anymore
    // Start is called before the first frame update
    void Start()
    {
        //Initialize all Buttons, X is currently without function and not used
        Button_Left = GameObject.Find("Button_Left");
        Button_Right = GameObject.Find("Button_Right");
        Button_A = GameObject.Find("Button_A");
        Button_B = GameObject.Find("Button_B");
        Button_X = GameObject.Find("Button_X");
        Button_Y = GameObject.Find("Button_Y");
        Button_L = GameObject.Find("Button_L");
        Button_R = GameObject.Find("Button_R");

        //Subscribe methods to events in the Start-method
        PlayerInput.onHorizontalDown += Arrows;
        PlayerInput.onJumpButtonDown += A_Button;
        PlayerInput.onPlayerAttackDown += B_Button;
        PlayerInput.onSlowMoDown += Y_Button;
        PlayerInput.onTiltDown += SchultertastenDown;
  
       


        PlayerInput.onJumpButtonUp += A_ButtonUp;
        PlayerInput.onPlayerAttackUp += B_ButtonUp;
        PlayerInput.onSlowMoUp += Y_ButtonUp;
        PlayerInput.onHorizontalUp += ArrowsUp;
        PlayerInput.onTiltUp += SchultertastenUp;
     





    }
    private void Update()
    {
       

    }

    private void OnDisable()
    {
        //Unsubscribing methods from events, when this component is disabled or destroyed.
        //This is important, otherwise the event will try to call methods, that are eventually not accessable anymore and throw NullExceptions
        PlayerInput.onHorizontalDown -= Arrows;
        PlayerInput.onJumpButtonDown -= A_Button;
        PlayerInput.onPlayerAttackDown -= B_Button;
        PlayerInput.onSlowMoDown -= Y_Button;
        PlayerInput.onTiltDown -= SchultertastenDown;
        

        PlayerInput.onJumpButtonUp -= A_ButtonUp;
        PlayerInput.onPlayerAttackUp -= B_ButtonUp;
        PlayerInput.onSlowMoUp -= Y_ButtonUp;
        PlayerInput.onHorizontalUp -= ArrowsUp;
        PlayerInput.onTiltUp -= SchultertastenUp;
       


    }

    //Following are the methods that actually change the color of the pressed button
    private void Arrows(float horizontal)
    {
        if (horizontal < 0)
        {
            Button_Left.GetComponent<Image>().color = highlighted;
        }
        else if (horizontal > 0)
        {
            Button_Right.GetComponent<Image>().color = highlighted;
        }
        
    }

    private void ArrowsUp(float horizontal)
    {
        Button_Left.GetComponent<Image>().color = dark;
        Button_Right.GetComponent<Image>().color = dark;
    }

    private void A_Button()
    {
        Button_A.GetComponent<Image>().color = highlighted;
    }

    private void A_ButtonUp()
    {
        Button_A.GetComponent<Image>().color = dark;
    }


    private void B_Button()
    {
        Button_B.GetComponent<Image>().color = highlighted;
    }
    private void B_ButtonUp()
    {
        Button_B.GetComponent<Image>().color = dark;
    }

    private void Y_Button()
    {
        Button_Y.GetComponent<Image>().color = highlighted;
    }

    private void Y_ButtonUp()
    {
        Button_Y.GetComponent<Image>().color = dark;
    }

    private void SchultertastenDown(float direction)
    {
        if (direction < 0)
        {
            Button_L.GetComponent<Image>().color = highlighted;
        }
        if (direction > 0)
        {
            Button_R.GetComponent<Image>().color = highlighted;
        }
    }

    private void SchultertastenUp(float direction)
    {
        Button_L.GetComponent<Image>().color = dark;
        Button_R.GetComponent<Image>().color = dark;
    }

    
}

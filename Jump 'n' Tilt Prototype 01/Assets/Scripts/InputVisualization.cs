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

    public GameObject Button_Left;
    public GameObject Button_Right;
    public GameObject Button_Down;
    public GameObject Button_Up;
    public GameObject Button_A;
    public GameObject Button_B;
    public GameObject Button_X;
    public GameObject Button_Y;
    public GameObject Button_L;
    public GameObject Button_R;
 

    private Color highlighted = new Color(1f, 1f, 1f); //Color for the highlighted Buttons
    private Color dark = new Color(0.79f, 0.79f, 0.79f); //Color if the Buttons are not pressed anymore


    // Start is called before the first frame update
    void Start()
    {

        //Subscribe methods to events in the Start-method for pressing a button
        PlayerInput.onHorizontalDown += HorizontalArrowsDown;
        PlayerInput.onJumpButtonDown += A_ButtonDown;
        PlayerInput.onPlayerAttackDown += B_ButtonDown;
        PlayerInput.onSlowMoDown += Y_ButtonDown;
        PlayerInput.onTiltDown += SchultertastenDown;
        PlayerInput.onVerticalDown += VerticalArrowsDown;


        //Subscribe methods to events in the Start-method for releasing a button
        PlayerInput.onJumpButtonUp += A_ButtonUp;
        PlayerInput.onPlayerAttackUp += B_ButtonUp;
        PlayerInput.onSlowMoUp += Y_ButtonUp;
        PlayerInput.onHorizontalUp += HorizontalArrowsUp;
        PlayerInput.onTiltUp += SchultertastenUp;
        PlayerInput.onVerticalUp += VerticalArrowsUp;






    }
    private void Update()
    {
       

    }

    private void OnDisable()
    {
        //Unsubscribing methods from events, when this component is disabled or destroyed.
        //This is important, otherwise the event will try to call methods, that are eventually not accessable anymore and throw NullExceptions
        PlayerInput.onHorizontalDown -= HorizontalArrowsDown;
        PlayerInput.onJumpButtonDown -= A_ButtonDown;
        PlayerInput.onPlayerAttackDown -= B_ButtonDown;
        PlayerInput.onSlowMoDown -= Y_ButtonDown;
        PlayerInput.onTiltDown -= SchultertastenDown;
        PlayerInput.onVerticalDown += VerticalArrowsDown;


        PlayerInput.onJumpButtonUp -= A_ButtonUp;
        PlayerInput.onPlayerAttackUp -= B_ButtonUp;
        PlayerInput.onSlowMoUp -= Y_ButtonUp;
        PlayerInput.onHorizontalUp -= HorizontalArrowsUp;
        PlayerInput.onTiltUp -= SchultertastenUp;
        PlayerInput.onVerticalUp -= VerticalArrowsUp;



    }

    //Following are the methods that actually change the color of the pressed button


    //Function to highlight the Left and Right Arrow
    private void HorizontalArrowsDown(float horizontal)
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

    //Function to dark the Left and Right Arrow
    private void HorizontalArrowsUp(float horizontal)
    {
        Button_Left.GetComponent<Image>().color = dark;
        Button_Right.GetComponent<Image>().color = dark;
    }

    //Function to highlight the A Button
    private void A_ButtonDown()
    {
        Button_A.GetComponent<Image>().color = highlighted;
    }

    //Function to dark the A Button
    private void A_ButtonUp()
    {
        Button_A.GetComponent<Image>().color = dark;
    }

    //Function to highlight the B Button
    private void B_ButtonDown()
    {
        Button_B.GetComponent<Image>().color = highlighted;
    }

    //Function to dark the B Button
    private void B_ButtonUp()
    {
        Button_B.GetComponent<Image>().color = dark;
    }

    //Function to highlight the Y Button
    private void Y_ButtonDown()
    {
        Button_Y.GetComponent<Image>().color = highlighted;
    }

    //Function to dark the Y Button
    private void Y_ButtonUp()
    {
        Button_Y.GetComponent<Image>().color = dark;
    }

    //Function to highlight the L and R Buttons (Schultertasten)
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

    //Function to dark the L and R Buttons (Schultertasten)

    private void SchultertastenUp(float direction)
    {
        Button_L.GetComponent<Image>().color = dark;
        Button_R.GetComponent<Image>().color = dark;
    }

    //Function to highlight the Up and Down Arrow
    private void VerticalArrowsDown(float vertical)
    {
        if(vertical < 0)
        {
            Button_Down.GetComponent<Image>().color = highlighted;
        }
        if(vertical > 0)
        {
            Button_Up.GetComponent<Image>().color = highlighted;
        }
        
    }

    //Function to dark the Up and Down Arrow
    private void VerticalArrowsUp(float vertical)
    {
        
            Button_Up.GetComponent<Image>().color = dark;
            Button_Down.GetComponent<Image>().color = dark;
      
    }

}

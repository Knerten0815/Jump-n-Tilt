using UnityEngine;

//Author: Kevin Zielke, Michelle Limbach
namespace GameActions //To access the PlayerInput add: "using GameActions;" at the top
{
    /// <summary>
    /// This class throws events for all actions the player wants to perform. It DOES NOT check if the player is able to perform the action.
    /// Virtual buttons and axes can be set in Unity via the Input Manager (Edit / Project Settings / Input Manager)
    /// Please remember to write control-alterations in the Input Manager into the Controls.docx in Goggle Drive and into the comments here. 
    /// </summary>
    public class PlayerInput : MonoBehaviour
    {

        ////////////////////////////////////////////////Navigation Input for Game and Menus\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public delegate void horizontalDown(float horizontal);      //Player wants to navigate or navigate horizontally: negative float for left or
        public static event horizontalDown onHorizontalDown;        //positive float for right. Uses "Horizontal" axis

        public delegate void horizontalUp(float horizontal);  
        public static event horizontalUp onHorizontalUp;        

        public delegate void verticalDown (float vertical);         //Player wants to navigate or navigate vertically: negative float for left or 
        public static event verticalDown onVerticalDown;            //positive float for right. Uses "Vertical" axis

        public delegate void verticalUp(float vertical);            //Player wants to navigate or navigate vertically: negative float for left or 
        public static event verticalUp onVerticalUp;                //positive float for right. Uses "Vertical" axis

        ///////////////////////////////////////////////////////////Game Input\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public delegate void jumpButtonDown();                      //Player presses jump Button
        public static event jumpButtonDown onJumpButtonDown;        //uses "Jump" button

        public delegate void jumpButtonUp();                        //Player releases jump Button
        public static event jumpButtonUp onJumpButtonUp;            //uses "Jump" button

        public delegate void playerAttackDown();                    //Player wants to perform a basic attack
        public static event playerAttackDown onPlayerAttackDown;    //uses "Attack" button

        public delegate void playerAttackUp();                      //Player releases basic attack button
        public static event playerAttackUp onPlayerAttackUp;        //uses "Attack" button

        public delegate void tiltDown(float direction);             //Player wants to tilt the wolrd: -1 for left or 1 for right
        public static event tiltDown onTiltDown;                    //uses "Tilt" axis

        public delegate void tiltUp(float direction);               //Player releases "Tilt" Button
        public static event tiltUp onTiltUp;                        //uses "Tilt" axis

        public delegate void slowMoDown();                          //Player wants to slow down Time
        public static event slowMoDown onSlowMoDown;                //uses "SlowMo" button

        public delegate void slowMoUp();                            //Player releases "SlowMo" button
        public static event slowMoUp onSlowMoUp;                    //uses "SlowMo" button

        public delegate void stomp();                               //Player wants to perform a stomp-attack
        public static event stomp onStomp;                          //uses "Vertical" axis and "Attack" or "Tilt" buttons

    

        //////////////////////////////////////////////////////////Menu Input\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public delegate void submitDown();                          //Player wants to submit a selection
        public static event submitDown onSubmitDown;                //uses "Submit" button

        public delegate void cancelDown();                          //Player wants to cancel
        public static event cancelDown onCancelDown;                //uses "Cancel" button

        public delegate void menuDown();                            //Player wants to open the Menu or get one step higher in Menu Hierarchy
        public static event menuDown onMenuDown;                    //uses "Menu" button


        //checks for Player Inputs
        void Update()
        {
            ////////////////////////////////////////////////Navigation Input for Game and Menus\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            
            //horizontal navigation and movement: A/D, left/right arrows on keyboard; left joystick on gamepad
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                onHorizontalDown?.Invoke(Input.GetAxis("Horizontal"));
            }
            else if(Input.GetAxisRaw("Horizontal") == 0)
            {
                onHorizontalUp?.Invoke(Input.GetAxis("Horizontal"));
            }

            //vertical navigation and movement: W/S, up/down arrows on keyboard; left joystick on gamepad
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                onVerticalDown?.Invoke(Input.GetAxis("Vertical"));
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                onVerticalUp?.Invoke(Input.GetAxis("Vertical"));
            }

            ///////////////////////////////////////////////////////////Game Input\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

            //jump: space on keyboard; A on gamepad
            if (Input.GetButtonDown("Jump"))
            {
                onJumpButtonDown?.Invoke();
            }
            else if (Input.GetButtonUp("Jump"))
            {
                onJumpButtonUp?.Invoke();
            }

            //stomp-attack: S, down-arrow on keyboard; left joystick on gamepad AND ("Attack" button OR "Tilt" axis)
            if (Input.GetAxisRaw("Vertical") < 0 && (Input.GetButtonDown("Attack") || Input.GetButtonDown("Tilt")))
            {
                onStomp?.Invoke();
            }
           
            else    //important not to throw stomp events and attack or tilt events at the same time
            {
                //basic attack: left mouse-button; B on gamepad
                if (Input.GetButtonDown("Attack"))
                {
                    onPlayerAttackDown?.Invoke();
                }
                else if (Input.GetButtonUp("Attack"))
                {
                    onPlayerAttackUp?.Invoke();
                }

                //Tilt: Q/E on keyboard; LB/RB on Gamepad
                if (Input.GetButtonDown("Tilt"))
                {
                    onTiltDown?.Invoke(Input.GetAxisRaw("Tilt"));
                }
                else if (Input.GetButtonUp("Tilt"))
                {
                    onTiltUp?.Invoke(Input.GetAxisRaw("Tilt"));
                }
            }

            //Slow Down Time: LeftShift on keyboard; Y on Gamepad
            if (Input.GetButtonDown("SlowMo"))
            {
                onSlowMoDown?.Invoke();
            }
            else if (Input.GetButtonUp("SlowMo"))
            {
                onSlowMoUp?.Invoke();
            }

            //////////////////////////////////////////////////////////Menu Input\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

            //Submit a selection: enter, return on keyboard; A on gamepad
            if (Input.GetButtonDown("Submit"))
            {
                onSubmitDown?.Invoke();
            }

            //Cancel a menu: backspace on keyboard, B on gamepad
            if (Input.GetButtonDown("Cancel"))
            {
                onCancelDown?.Invoke();
            }

            //open up Menu or move one menu higher: esc on keyboard, Start on gamepad
            if (Input.GetButtonDown("Menu"))
            {
                onMenuDown?.Invoke();
            }

        }
    }
}

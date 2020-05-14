using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Kevin Zielke
namespace GameActions //To access the PlayerInput add: "using GameActions;" at the top
{
    public class PlayerInput : MonoBehaviour
    {
        //This class throws events for all actions the player wants to perform. It DOES NOT check if the player is able to perform the action.
        //Virtual buttons and axes can be set in Unity via the Input Manager (Edit / Project Settings / Input Manager)
        //Please remember to write control-alterations in the Input Manager into the Controls.docx in Goggle Drive and into the comments here. 

        ////////////////////////////////////////////////Navigation Input for Game and Menus\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public delegate void horizontal(float horizontal);  //Player wants to navigate or navigate horizontally: negative float for left or
        public static event horizontal onHorizontal;        //positive float for right. Uses "Horizontal" axis

        public delegate void vertical (float horizontal);   //Player wants to navigate or navigate vertically: negative float for left or 
        public static event vertical onVertical;            //positive float for right. Uses "Vertical" axis

        ///////////////////////////////////////////////////////////Game Input\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public delegate void jump();                        //Player wants to jump
        public static event jump onJump;                    //uses "Jump" button

        public delegate void playerAttack();                //Player wants to perform a basic attack
        public static event playerAttack onPlayerAttack;    //uses "Attack" button

        public delegate void tilt(float direction);         //Player wants to tilt the wolrd: -1 for left or 1 for right
        public static event tilt onTilt;                    //uses "Tilt" axis

        public delegate void slowMo();                      //Player wants to slow down Time
        public static event slowMo onSlowMo;                //uses "SlowMo" button

        public delegate void stomp();                       //Player wants to perform a stomp-attack
        public static event stomp onStomp;                  //uses "Vertical" axis and "Attack" or "Tilt" buttons

        //////////////////////////////////////////////////////////Menu Input\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        public delegate void submit();                      //Player wants to submit a selection
        public static event submit onSubmit;                //uses "Submit" button

        public delegate void cancel();                      //Player wants to cancel
        public static event cancel onCancel;                //uses "Cancel" button

        public delegate void menu();                        //Player wants to open the Menu or get one step higher in Menu Hierarchy
        public static event menu onMenu;                    //uses "Menu" button


        //checks for Player Inputs
        void Update()
        {
            ////////////////////////////////////////////////Navigation Input for Game and Menus\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            
            //horizontal movement: A/D, left/right arrows on keyboard; left joystick on gamepad
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                onHorizontal?.Invoke(Input.GetAxis("Horizontal"));
            }

            //horizontal movement: A/D, left/right arrows on keyboard; left joystick on gamepad
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                onVertical?.Invoke(Input.GetAxis("Vertical"));
            }

            ///////////////////////////////////////////////////////////Game Input\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
            
            //jumping: space on keyboard; A on gamepad
            if (Input.GetButton("Jump"))
            {
                onJump?.Invoke();
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
                    onPlayerAttack?.Invoke();
                }

                //Tilt: Q/E on keyboard; LB/RB on Gamepad
                if (Input.GetButtonDown("Tilt"))
                {
                    onTilt?.Invoke(Input.GetAxisRaw("Tilt"));
                }
            }

            //Slow Down Time: LeftShift on keyboard; Y on Gamepad
            if (Input.GetButton("SlowMo"))
            {
                onSlowMo?.Invoke();
            }

            //////////////////////////////////////////////////////////Menu Input\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

            //Submit a selection: enter, return on keyboard; A on gamepad
            if (Input.GetButton("Submit"))
            {
                onSubmit?.Invoke();
            }

            //Cancel a menu: backspace on keyboard, B on gamepad
            if (Input.GetButton("Cancel"))
            {
                onCancel?.Invoke();
            }

            //open up Menu or move one menu higher: esc on keyboard, Start on gamepad
            if (Input.GetButton("Menu"))
            {
                onMenu?.Invoke();
            }
        }
    }
}

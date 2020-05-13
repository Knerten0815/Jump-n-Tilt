using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameActions //To access the PlayerInput add: "using GameActions;" at the top
{
    public class PlayerInput : MonoBehaviour
    {
        //Author: Kevin Zielke
        //This class throws events for all in-game-actions the player wants to perform. It DOES NOT check if the player is able to perform the action.
        //Buttons and Axes can be set in Unity via the Input Manager (Edit / Project Settings / Input Manager)
        //Please remember to write alterations in the Input Manager into the comments here. 

        public delegate void move(float horizontal);        //Player wants to move horizontally: negative float for left or positive float for right
        public static event move onMove;                    //uses "Horizontal" axis

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


        //checks for Player Inputs
        void Update()
        {
            //horizontal movement: A/D, left/right arrows on keyboard; left joystick on gamepad
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                onMove?.Invoke(Input.GetAxis("Horizontal"));
            }
            //jumping: space on keyboard; A on gamepad
            if (Input.GetButton("Jump"))
            {
                onJump?.Invoke();
            }
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
            //Slow Down Time: LeftShift on keyboard; Y on Gamepad
            if (Input.GetButton("SlowMo"))
            {
                onSlowMo?.Invoke();
            }
            //stomp-attack: S, down-arrow on keyboard; left joystick on gamepad AND ("Attack" button OR "Tilt" axis)
            if (Input.GetAxisRaw("Vertical") < 0 && (Input.GetButtonDown("Attack") || Input.GetButtonDown("Tilt")))
            {
                onStomp?.Invoke();
            }
        }
    }
}

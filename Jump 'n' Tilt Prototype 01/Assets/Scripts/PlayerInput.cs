using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    //Author: Kevin Zielke
    //This class throws events for all actions the player wants to perform. It DOES NOT check if the player is able to perform the action.
    //Buttons and Axes can be set in Unity via the Input Manager (Edit / Project Settings / Input Manager)

    public delegate void move(float horizontal);        //Player wants to move horizontally: returns negative float for left or positive float for right
    public static event move onMove;                    //uses "Horizontal" axis

    public delegate void jump();                        //Player wants to jump
    public static event jump onJump;                    //uses "Jump" button

    public delegate void playerAttack();                //Player wants to perform a basic attack
    public static event playerAttack onPlayerAttack;    //uses "Attack" button

    public delegate void tilt(float direction);         //Player wants to tilt the wolrd: returns -1 for left or 1 for right
    public static event tilt onTilt;                    //uses "Tilt" axis

    public delegate void slowMo();                      //Player wants to slow down Time
    public static event slowMo onSlowMo;                //uses "SlowMo" button

    public delegate void stomp();                       //Player wants to perform a stomp-attack
    public static event stomp onStomp;                  //uses "Vertical" axis and "Attack" or "Tilt" buttons


    //checks for Player Inputs
    void Update()
    {
        //horizontal movement: A/D, left/right arrows, x-axis on joysticks
        if(Input.GetAxisRaw("Horizontal") != 0)
        {
            onMove?.Invoke(Input.GetAxis("Horizontal"));
            Debug.Log("horizontal: " + Input.GetAxis("Horizontal"));
        }
        //stomp-attack if player presses downwards-movement AND (Attack OR Tilt)
        else if (Input.GetAxisRaw("Vertical") < 0 && (Input.GetButtonDown("Attack") || Input.GetButtonDown("Tilt")))
        {
            onStomp?.Invoke();
            Debug.Log("stomp");
        }
        //jumping
        else if (Input.GetButton("Jump"))
        {
            onJump?.Invoke();
            Debug.Log("jump");
        }
        //basic attack
        else if (Input.GetButtonDown("Attack"))
        {
            onPlayerAttack?.Invoke();
            Debug.Log("attack");
        }
        //Tilt
        else if (Input.GetButtonDown("Tilt"))
        {
            onTilt?.Invoke(Input.GetAxisRaw("Tilt"));
            Debug.Log("tilt: " + Input.GetAxis("Tilt"));
        }
        //Slow Down Time
        else if (Input.GetButton("SlowMo"))
        {
            onSlowMo?.Invoke();
            Debug.Log("slowmo");
        }        
    }
}

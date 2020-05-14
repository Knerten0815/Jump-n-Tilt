using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Kevin Zielke
//This class is for testing and learning event behaviour
//The events of this class are initalized in PlayerInput.cs

using GameActions; //needs to be added to access the PlayerInput events
public class InputTest : MonoBehaviour
{
    
    void Start()
    {
        //subscribing methods to events in the Start-method
        PlayerInput.onHorizontal += Move;
        //PlayerInput.onJump += Jump;
        PlayerInput.onPlayerAttack += Attack;
        PlayerInput.onTilt += Tilt;
        PlayerInput.onSlowMo += SlowMo;
        PlayerInput.onStomp += Stomp;        
    }

    private void OnDisable()
    {
        //unsubscribing methods from events, when this component is disabled or destroyed.
        //This is important, otherwise the event will try to call methods, that are eventually not accessable anymore and throw NullExceptions
        PlayerInput.onHorizontal -= Move;
        //PlayerInput.onJump -= Jump;
        PlayerInput.onPlayerAttack -= Attack;
        PlayerInput.onTilt -= Tilt;
        PlayerInput.onSlowMo -= SlowMo;
        PlayerInput.onStomp -= Stomp;
    }

    //the subscribed methods contain the actual code, that should be executed, when the corresponding event is invoked
    private void Move(float horizontal)
    {
        Debug.Log("horizontal: " + horizontal);
    }

    private void Jump()
    {
        Debug.Log("jump");
    }
    private void Attack()
    {
        Debug.Log("attack");
    }

    private void Tilt(float direction)
    {
        Debug.Log("tilt: " + direction);
    }

    private void SlowMo()
    {
        Debug.Log("slowmo");
    }
    private void Stomp()
    {
        Debug.Log("stomp");
    }
}

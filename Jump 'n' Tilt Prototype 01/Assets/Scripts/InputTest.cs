using GameActions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput.onMove += Move;
        PlayerInput.onJump += Jump;
        PlayerInput.onPlayerAttack += Attack;
        PlayerInput.onTilt += Tilt;
        PlayerInput.onSlowMo += SlowMo;
        PlayerInput.onStomp += Stomp;        
    }

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

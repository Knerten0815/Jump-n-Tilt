using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;
using TimeControlls;

public class LevelControllerNew : MonoBehaviour
{
    //angle and speed can be changed in the editor
    public float tiltAngle = 22.5f; 
    public float tiltSpeed = 10f;
    public float tiltBackSpeed = 2f;

    private Transform player;
    private Transform level;

    private Vector3 currentTilt;
    private int tiltStep;

    private float playerInput;
    private float tiltTime;

    private TimeController timeController;

    private bool isAxisInUse = false;
    private bool tiltRight = false;
    private bool tiltLeft = false;

    private float targetRight;
    private float startRight;

    private float targetLeft;
    private float startLeft;

    private void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        level = GameObject.Find("Level").transform;
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    private void Start()
    {
        currentTilt = transform.eulerAngles;
        tiltStep = 0;

        PlayerInput.onTiltDown += TiltMechanic;
    }

    private void OnDisable()
    {
        PlayerInput.onTiltDown -= TiltMechanic;
    }

    private void Update()
    {
        playerInput = Input.GetAxisRaw("Tilt");
        TiltMechanic(playerInput);  
    }


    private void TiltMechanic(float playerInput)
    {
        if (playerInput == 0)
        {
            isAxisInUse = false;
        }

        if (playerInput == 1 && (tiltStep == 0 || tiltStep == -1) && isAxisInUse == false)
        {
            tiltRight = true;
            tiltTime = 0;
            isAxisInUse = true;
            tiltStep++;

            targetRight = transform.eulerAngles.z + tiltAngle;
            startRight = transform.eulerAngles.z;
        }

        if(playerInput == -1 && (tiltStep == 0 || tiltStep == 1) && isAxisInUse == false)
        {
            tiltLeft = true;
            tiltTime = 0;
            isAxisInUse = true;
            tiltStep--;

            targetLeft = transform.eulerAngles.z + (-tiltAngle);
            startLeft = transform.eulerAngles.z;
        }

        

        if(tiltRight == true)
        {
            RightTilt();
       
            if(tiltTime > 1)
            {
                tiltRight = false;
            }
        }

        if(tiltLeft == true)
        {
            LeftTilt();

            if(tiltTime > 1)
            {
                tiltLeft = false;
            }
        }
    }

 
    private void RightTilt()
    {
        setWorldPosition();
        setWorldParent();

        tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

        float rightStep = Mathf.LerpAngle(startRight, targetRight, tiltTime);
        Vector3 rightIncrement = new Vector3(0, 0, rightStep);
        transform.eulerAngles = rightIncrement;

        currentTilt = transform.eulerAngles;
        
        unsetWorldParent();
    }

    private void LeftTilt()
    {
        setWorldPosition();
        setWorldParent();

        tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

        float leftStep = Mathf.LerpAngle(startLeft, targetLeft, tiltTime);
        Vector3 leftIncrement = new Vector3(0, 0, leftStep);
        transform.eulerAngles = leftIncrement;

        currentTilt = transform.eulerAngles;

        unsetWorldParent();
    }











    //Author: Melanie Jäger
    //sets the position for the rotation so that the rotation always goes aroung the player
    private void setWorldPosition()
    {
        transform.position = player.transform.position;
    }

    //Author: Melanie Jäger
    //sets LevelController as parent for Level
    private void setWorldParent()
    {
        level.transform.SetParent(transform);
    }

    //Author: Melanie Jäger
    //unsets LevelController as parent for Level
    private void unsetWorldParent()
    {
        level.transform.SetParent(null);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;
using TimeControlls;

//Author: Melanie Jäger
public class LevelControllerNew : MonoBehaviour
{
    //public variables can be changed in the editor
    public float tiltAngle = 22.5f;     //angle that the world rotates in one step
    public float numTiltStep = 1f;      //max number of tilts in each direction
    public int defaultTilt = 0;
    public float tiltSpeed = 10f;       //speed of the playertilt
    public float tiltBackSpeed = 5f;    //speed of the automatic backtilt
    public float delayTime = 0.5f;      //time before the automatic backtilt starts

    private Transform player;
    private Transform level;

    private int tiltStep;               //counter variable for the tilt in each direction

    private float playerInput;
    private float tiltTime;
    private float tiltBackTime;
    private float delay;

    private TimeController timeController;

    private bool isAxisInUse = false;   //becomes true when a button for tilting is pressed; coordinates which tilting options are currently available
    private bool tiltRight = false;     //becomes true when E-Button/RB is pressed
    private bool tiltLeft = false;      //becomes true when Q-Button/LB is pressed
    private bool tiltBack = false;      //becomes true when no button is pressed
    private bool canTilt = true;        //becomes false when BackTilt() is active; prevents from tilting during the backtilt

    private float playerPos;

    private float targetRight;
    private float startRight;

    private float targetLeft;
    private float startLeft;

    //Author: Melanie Jäger
    //gets all the needed objects from scene and other scripts
    private void OnEnable()
    {
        player = GameObject.Find("Player").transform;
        level = GameObject.Find("Level").transform;
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    //Author: Melanie Jäger
    private void Start()
    {
        tiltStep = defaultTilt;                           //sets the default rotation as step = 0
        PlayerInput.onTiltDown += TiltMechanic;
    }

    //Author: Melanie Jäger
    private void OnDisable()
    {
        PlayerInput.onTiltDown -= TiltMechanic;
    }

    //Author: Melanie Jäger
    private void Update()
    {
        playerInput = Input.GetAxisRaw("Tilt");
        TiltMechanic(playerInput);  
    }

    //Author: Melanie Jäger
    //method is called when the tilt event is triggered
    private void TiltMechanic(float playerInput)
    {
        //called when no button is pressed and a tilt happened beforehand
        if (playerInput == 0 && isAxisInUse == true)
        {
            tiltBackTime = 0f;
            delay = -2f;
            tiltBack = true;
            isAxisInUse = false;

            playerPos = transform.eulerAngles.z;
        }

        //called when E/RB is pressed, max step to the right is not reached, not tilt is currently activ and BackTilt() not activ
        if (playerInput == 1 && tiltStep != numTiltStep && isAxisInUse == false && canTilt == true)
        {
            tiltTime = 0;
            tiltRight = true;
            isAxisInUse = true;
            tiltStep++;

            targetRight = transform.eulerAngles.z + tiltAngle;
            startRight = transform.eulerAngles.z;
        }

        //called when Q/LB is pressed, max step to the right is not reached, not tilt is currently activ and BackTilt() not activ
        if (playerInput == -1 && tiltStep != -numTiltStep && isAxisInUse == false && canTilt == true)
        {
            tiltTime = 0;
            tiltLeft = true;
            isAxisInUse = true;
            tiltStep--;

            targetLeft = transform.eulerAngles.z + (-tiltAngle);
            startLeft = transform.eulerAngles.z;
        }

        //called when the conditions for a backtilt are met
        if(tiltBack == true)
        {
            delay += delayTime * timeController.getSpeedAdjustedDeltaTime();

            //starts tilting back when the delay has reached 0
            if(delay > 0f)
            {
                BackTilt();
                canTilt = false;    //prevents player from tilting when BackTilt() is currently processing

                if(tiltBackTime > 1)
                {
                    tiltBack = false;       //becomes false when BackTilt() is finished; prevents from calling the method continuously
                    tiltStep = defaultTilt; //tiltStep goes back to the default rotation
                    canTilt = true;         //after tilting back the player should be able to tilt again normally
                }
            }
        }

        //called when the conditions for a righttilt are met
        if (tiltRight == true)
        {
            RightTilt();
       
            if(tiltTime > 1)
            {
                tiltRight = false;      //becomes false when RightTilt() is finished; prevents from calling the method continuously
            }
        }

        //called when the conditions for a lefttilt are met
        if (tiltLeft == true)
        {
            LeftTilt();

            if(tiltTime > 1)
            {
                tiltLeft = false;       //becomes false when LeftTilt() is finished; prevents from calling the method continuously
            }
        }
    }

    //Author: Melanie Jäger
    //tilts the world back to the default rotation when the delay counter runs out
    private void BackTilt()
    {
        setWorldPosition();  //tilt goes always around the player
        setWorldParent();

        tiltBackTime += tiltBackSpeed * timeController.getSpeedAdjustedDeltaTime();

        float backStep = Mathf.LerpAngle(playerPos, 0f, tiltBackTime);          //interpolates between the start and the endposition for a smooth tilting
        Vector3 backTilt = new Vector3(0, 0, backStep);
        transform.eulerAngles = backTilt;

        unsetWorldParent(); //prevents whole world from changing the position as well
    }

    //Author: Melanie Jäger
    //tilts the world to the right
    private void RightTilt()
    {
        setWorldPosition();     //tilt goes always around the player
        setWorldParent();

        tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

        float rightStep = Mathf.LerpAngle(startRight, targetRight, tiltTime);   //interpolates between the start and the endposition for a smooth tilting
        Vector3 rightIncrement = new Vector3(0, 0, rightStep);
        transform.eulerAngles = rightIncrement;
        
        unsetWorldParent();     //prevents whole world from changing the position as well
    }

    //Author: Melanie Jäger
    //tilts the world to the left
    private void LeftTilt()
    {
        setWorldPosition();     //tilt goes always around the player
        setWorldParent();

        tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

        float leftStep = Mathf.LerpAngle(startLeft, targetLeft, tiltTime);      //interpolates between the start and the endposition for a smooth tilting
        Vector3 leftIncrement = new Vector3(0, 0, leftStep);
        transform.eulerAngles = leftIncrement;

        unsetWorldParent();     //prevents whole world from changing the position as well
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

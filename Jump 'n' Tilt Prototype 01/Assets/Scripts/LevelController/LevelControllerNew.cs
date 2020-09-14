using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;
using TimeControlls;

//Author: Melanie Jäger
//Minor changes by Marvin Winkler
//Class for tilting the level
namespace LevelControlls
{
    public class LevelControllerNew : MonoBehaviour
    {
        //public variables can be changed in the editor
        public float tiltAngle = 22.5f;     //angle that the world rotates in one step
        public float tiltSpeed = 10f;       //speed of the playertilt
        public float tiltBackSpeed = 5f;    //speed of the automatic backtilt

        //Variables for the gameobjects
        private Transform player;
        private Transform level;
        private TimeController timeController;

        //Variables that control the tilt
        public float numTiltStep = 1f;      //max number of tilts in each direction
        public int defaultTilt = 0;         //default value for the tilt direction; 0 = no rotation
        private int tiltStep;               //Variable to prove the current tilt direction, 0 = default position
        private float tiltTime;             //time the tilt takes until the end position is reached
        private float tiltBackTime;         //time the backtilt takes until the end position is reached
        private bool isAxisInUse = false;   //becomes true when a button for tilting is pressed; coordinates which tilting options are currently available
        private bool tiltRight = false;     //becomes true when E-Button/RB is pressed
        private bool tiltLeft = false;      //becomes true when Q-Button/LB is pressed
        private bool tiltBack = false;      //becomes true when no button is pressed
        private bool canTilt = true;        //becomes false when BackTilt() is active; prevents from tilting during the backtilt

        //Start and end points for the tilt
        private float targetRight;
        private float startRight;
        private float targetLeft;
        private float startLeft;
        private float playerPos;

        public delegate void worldWasTilted(float tiltDirection);
        public static event worldWasTilted onWorldWasTilted;

        public delegate void worldWasUntilted();
        public static event worldWasUntilted onWorldWasUntilted;

        //gets all the needed objects from scene and other scripts
        private void OnEnable()
        {
            player = GameObject.Find("Player").transform;
            level = GameObject.Find("Level").transform;
            timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        }

        private void Start()
        {
            tiltStep = defaultTilt;                                 //sets the default rotation as step = 0
            PlayerCharacter.onFishCausedEarthquake += TiltMechanic; //Changed by Marvin Winkler
        }

        private void OnDisable()
        {
            PlayerCharacter.onFishCausedEarthquake -= TiltMechanic; //Changed by Marvin Winkler
        }

        //method is called when the tilt event is triggered
        private void TiltMechanic(float playerInput)
        {
            //called when no button is pressed and a tilt happened beforehand
            if (playerInput == 0 && isAxisInUse == true)
            {
                tiltBackTime = 0f;
                tiltBack = true;
                isAxisInUse = false;

                //Start point for the back tilt
                playerPos = transform.eulerAngles.z;
            }

            //called when E/RB is pressed, world is not tilted to the right, no tilt is currently activ and BackTilt() is not activ
            if (playerInput == -1 && tiltStep != -numTiltStep && isAxisInUse == false && canTilt == true)
            {
                tiltTime = 0;
                tiltRight = true;
                tiltStep--;

                //Start and end point for the right tilt
                targetRight = transform.eulerAngles.z + tiltAngle;
                startRight = transform.eulerAngles.z;
            }

            //called when Q/LB is pressed, world is not tilted to the left, not tilt is currently activ and BackTilt() is not activ
            if (playerInput == 1 && tiltStep != numTiltStep && isAxisInUse == false && canTilt == true)
            {
                tiltTime = 0;
                tiltLeft = true;
                tiltStep++;

                //Start and end point for the left tilt
                targetLeft = transform.eulerAngles.z + (-tiltAngle);
                startLeft = transform.eulerAngles.z;
            }

            //called when the conditions for a backtilt are met
            if (tiltBack == true)
            {
                BackTilt();
                canTilt = false;            //prevents player from tilting when BackTilt() is currently processing

                if (tiltBackTime > 1)
                {
                    tiltBack = false;       //becomes false when BackTilt() is finished; prevents from calling the method continuously
                    tiltStep = defaultTilt; //tiltStep goes back to the default rotation
                    canTilt = true;         //after tilting back the player should be able to tilt again normally
                }
            }

            //called when the conditions for a righttilt are met
            if (tiltRight == true)
            {
                RightTilt();

                if (tiltTime > 1)
                {
                    tiltRight = false;      //becomes false when RightTilt() is finished; prevents from calling the method continuously
                    isAxisInUse = true;
                }
            }

            //called when the conditions for a lefttilt are met
            if (tiltLeft == true)
            {
                LeftTilt();

                if (tiltTime > 1)
                {
                    tiltLeft = false;       //becomes false when LeftTilt() is finished; prevents from calling the method continuously
                    isAxisInUse = true;
                }
            }
        }

        //tilts the world back to the default position
        private void BackTilt()
        {
            setWorldPosition();  
            setWorldParent();

            tiltBackTime += tiltBackSpeed * timeController.getSpeedAdjustedDeltaTime();

            float backStep = Mathf.LerpAngle(playerPos, 0f, tiltBackTime);          //interpolates between the start and the endposition for a smooth tilting
            Vector3 backTilt = new Vector3(0, 0, backStep);
            transform.eulerAngles = backTilt;

            onWorldWasUntilted?.Invoke();

            unsetWorldParent();
        }

        //tilts the world to the right
        private void RightTilt()
        {
            setWorldPosition();     
            setWorldParent();

            tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

            float rightStep = Mathf.LerpAngle(startRight, targetRight, tiltTime);   //interpolates between the start and the endposition for a smooth tilting
            Vector3 rightIncrement = new Vector3(0, 0, rightStep);
            transform.eulerAngles = rightIncrement;

            onWorldWasTilted?.Invoke(1f);

            unsetWorldParent();     
        }

        //tilts the world to the left
        private void LeftTilt()
        {
            setWorldPosition();     
            setWorldParent();

            tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

            float leftStep = Mathf.LerpAngle(startLeft, targetLeft, tiltTime);      //interpolates between the start and the endposition for a smooth tilting
            Vector3 leftIncrement = new Vector3(0, 0, leftStep);
            transform.eulerAngles = leftIncrement;

            onWorldWasTilted?.Invoke(-1f);

            unsetWorldParent();
        }

        //Sets the position for the rotation, so that the rotation always goes aroung the player
        private void setWorldPosition()
        {
            transform.position = player.transform.position;
        }

        //Sets LevelController as parent for Level, so that the level is tilted with the empty gameobject
        private void setWorldParent()
        {
            level.transform.SetParent(transform);
        }

        //Author: Marvin Winkler
        //Needed to fix wall climbing bug in player Character
        //Needed for the right movement for the arrows
        public int getTiltStep()
        {
            return tiltStep;
        }

        //unsets LevelController as parent for Level, so that the level does not move with the empty gameobject
        private void unsetWorldParent()
        {
            level.transform.SetParent(null);
        }
    }
}

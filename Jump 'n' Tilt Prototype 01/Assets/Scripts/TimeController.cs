using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;

namespace TimeControlls //To access the TimeController add: "using TimeControlls;" at the top
{
    public class TimeController : MonoBehaviour
    {
        //Author: Marvin Winkler
        //TimeController only works for Kinematic objects
        private float timeSpeed;                //Timespeed, 1 = real time, > 1 faster, < 1 slower
        private float speedAdjustedDeltaTime;   //Dela time adjusted for time speed
        private bool isSlow;                    //is time currently slow?
        public float slowTimeSpeed;     //1 is normal speed, < 1 is slower speed, > 1 faster speed

        public delegate void timeSpeedChange();
        public static event timeSpeedChange onTimeSpeedChange;  //Classes can subscribe to this event. It gets called when a time speed change accurs. Don't forget to unsubscribe on disable!

        //Author: Marvin Winkler
        private void Start()
        {
            PlayerCharacter.onUseSloMoTime += switchSloMo;
            timeSpeed = 1;
            isSlow = false;
        }

        //Author: Marvin Winkler
        private void OnDisable()
        {
            PlayerCharacter.onUseSloMoTime -= switchSloMo;
        }

        //Author: Marvin Winkler
        private void FixedUpdate()
        {
            if (isSlow)
                timeSpeed = slowTimeSpeed;

            timeAdjustments();
        }

        //Is subscribed to onSloMo switches timeSpeed
        //Author: Marvin Winkler
        private void switchSloMo()
        {
            if (isSlow)
            {
                timeSpeed = 1;
                isSlow = false;
            }
            else
            {
                timeSpeed = slowTimeSpeed;
                isSlow = true;
            }
        }

        //Author: Marvin Winkler
        private void timeAdjustments()
        {
            speedAdjustedDeltaTime = Time.deltaTime* timeSpeed; //Gets called in fixed update
        }

        //This should be called each time you would normally use deltaTime
        //Author: Marvin Winkler
        public float getSpeedAdjustedDeltaTime()
        {
            return speedAdjustedDeltaTime;
        }

        //Author: Marvin Winkler
        public float getTimeSpeed()
        {
            return timeSpeed;
        }

        //Author: Marvin Winkler
        public void setTimeSpeed(float timeSpeed)
        {
            this.timeSpeed = timeSpeed;
            onTimeSpeedChange?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeControlls //To access the TimeController add: "using TimeControlls;" at the top
{
    public class TimeController : MonoBehaviour
    {
        //Author: Marvin Winkler

        //TimeController only works for Kinematic objects

        private float timeSpeed;    //1 is normal speed, < 1 is slower speed, > 1 faster speed
        private float speedAdjustedDeltaTime;

        public delegate void timeSpeedChange();
        public static event timeSpeedChange onTimeSpeedChange;  //Classes can subscribe to this event. It gets called when a time speed change accurs. Don't forget to unsubscribe on disable!

        private void Start()
        {
            timeSpeed = 1;
        }

        //Author: Marvin Winkler
        void Update()
        {
            //Just for testing
            //++
            if (Input.GetKeyDown(KeyCode.T) && timeSpeed == 1)
            {
                setTimeSpeed(0.2f);
            }
            else if(Input.GetKeyDown(KeyCode.T) && timeSpeed == 0.2f)
            {
                setTimeSpeed(1);
            }
            //++

            timeAdjustments();
        }

        //Author: Marvin Winkler
        private void timeAdjustments()
        {
            speedAdjustedDeltaTime = Time.fixedDeltaTime * timeSpeed;
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
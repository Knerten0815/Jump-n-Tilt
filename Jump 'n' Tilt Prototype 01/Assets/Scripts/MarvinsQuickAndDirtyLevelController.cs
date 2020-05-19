using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TimeControlls;

public class MarvinsQuickAndDirtyLevelController : MonoBehaviour
{
    //Author: Marvin Winkler

        //This levelController is supposed to be temporary, it has a lot of stuff missing:

            //No gradually slowing down of tilt
            //No kinematic script driven movement while tilt is changing
            //No rotation around the player (tilt seems to be faster the further you go away from spawn)
            //No delay bevor tilting back to normal
            //only tilting back to normal when maxTilt is reached


    //public values can be changed in the editor

    public float maxRotation = 30;      
    public float rotationSpeed = 100;
    public float backRotationSpeed = 30;
    public float delay = 2;

    private float currentRotation;
    private Transform level;
    private float tiltDirection;
    private TimeController timeController;
    private float lastTiltDirection;

    //Author: Marvin Winkler
    private void OnEnable()
    {
        level = GameObject.Find("Level").transform;
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
    }

    //Author: Marvin Winkler
    // Start is called before the first frame update
    void Start()
    {
        tiltDirection = 0;
        currentRotation = 0;
    }

    //Author: Marvin Winkler
    // Update is called once per frame
    void Update()
    {
        if( Input.GetAxis("Tilt") != tiltDirection) //Does not use PlayerInput Class
        {
            tiltDirection = Input.GetAxis("Tilt");
        }
        if (Mathf.Abs(currentRotation) < 0.01f)
        {
            currentRotation = 0;
            lastTiltDirection = 0;
        }
        rotateLevel();
    }

    //Author: Marvin Winkler
    //Rotates Level in some direction
    private void rotateLevel()
    {
        if (Mathf.Abs(currentRotation) <= maxRotation)
        {
            currentRotation += (-tiltDirection * rotationSpeed * timeController.getSpeedAdjustedDeltaTime());
        }
        if (Mathf.Abs(currentRotation) > maxRotation)
        {
            currentRotation = (currentRotation / Mathf.Abs(currentRotation)) * maxRotation;
            lastTiltDirection = tiltDirection;
        }
        if (tiltDirection == 0)
        {
            currentRotation += lastTiltDirection * backRotationSpeed * timeController.getSpeedAdjustedDeltaTime();
        }

        level.localEulerAngles = new Vector3(0, 0, currentRotation);
    }
}

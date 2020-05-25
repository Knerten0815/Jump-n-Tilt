using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;
using TimeControlls;

public class LevelController : MonoBehaviour
{
    //Author: Melanie Jäger

    //public values can be changed in the editor
    
    public float tiltAngle = 10f;        //orientationvalue for incremental rotation, on avarage exact value is slightly bigger
    public float maxTilt = 0.35f;           //value für the number of Steps, 0.35 equals four steps in every direction with an angle of 11.25 degree
                                            //with 0.35 the angle is not going to be bigger than 45 degree
    public float tiltSpeed = 10f;         //speedvalue for the tilt
    public float tiltBackSpeed = 2f;    //speedvalue for the automatic backtilt

    private GameObject player;            //player object for the rotation center
    private float currentTilt;            //value that holds the current tilt

    private Transform level;
    private TimeController timeController;

    private bool isAxisInUse = false;
    private float tiltTime;                 
    private float tiltBackTime;
    private bool tiltRight = false;
    private bool tiltLeft = false;

    private Quaternion targetRight;
    private Quaternion targetLeft;

    //private Vector3 targetRight;
    //private Vector3 targetLeft;

    private void OnEnable()
    {
        level = GameObject.Find("Level").transform;
        timeController = GameObject.Find("TimeController").GetComponent<TimeController>();
        player = GameObject.Find("Player");                  

    }

    //Author: Melanie Jäger
    void Start()
    {
        currentTilt = transform.rotation.z;

        targetRight = new Quaternion();
        targetLeft = new Quaternion();

        //targetLeft = new Vector3();
        //targetRight = new Vector3();

        PlayerInput.onTiltDown += TiltMechanic;
        PlayerInput.onTiltUp += TiltBackMechanic;
    }

    private void OnDisable()
    {
        PlayerInput.onTiltDown -= TiltMechanic;
        PlayerInput.onTiltUp -= TiltBackMechanic;
    }

    //Author: Melanie Jäger
    void Update()
    {
        float playerInput = Input.GetAxisRaw("Tilt");
        TiltMechanic(playerInput);
        TiltBackMechanic(playerInput);

        Debug.Log(currentTilt);
    }

    //Author: Melanie Jäger
    //Method that is called when teh event is triggered
    private void TiltMechanic(float tiltDirection)
    {
        //tilt right if tiltDirection is 1, E-Button/RB is pressed
        if (tiltDirection > 0)
        {
            if (isAxisInUse == false)   //bool variable for single method calling
            {
                tiltRight = true;
                tiltTime = 0;
                isAxisInUse = true;
                targetRight = transform.rotation * Quaternion.Euler(new Vector3(0, 0, tiltAngle));
                //targetRight = transform.eulerAngles + new Vector3(0, 0, tiltAngle);
            }
        }

        if (tiltRight == true)
        {
            
            if (currentTilt >= 0 && currentTilt < maxTilt)  //rotate the world to the right when the current rotation equals or is bigger than 0 and the limit to maxTilt is not reached yet
            {
                TiltRight();
            }
            
            else if (currentTilt < 0)       //if the world is rotated to the left and the button for right rotation is pressed, the world immediately rotates to default
            {
                TiltBack();
            }
            
            if (currentTilt == 0 || currentTilt > maxTilt)  //stop tilting when the rotation is back to default or the maxTilt is reached
            {
                tiltRight = false;
            }
        }

        //tilt left if tiltDirection is -1, Q-Button/LB is pressed
        if (tiltDirection < 0)
        {
            if (isAxisInUse == false)   //bool variable for single method calling
            {
                tiltLeft = true;
                tiltTime = 0;
                isAxisInUse = true;
                targetLeft = transform.rotation * Quaternion.Euler(new Vector3(0, 0, -tiltAngle));
                //targetLeft = transform.eulerAngles + new Vector3(0, 0, -tiltAngle);
            }
        }

        if (tiltLeft == true)
        {
            
            if (currentTilt <= 0 && currentTilt > -maxTilt)   //rotate the world to the left when the current rotation equals or is smaller than 0 and the limit to -maxTilt is not reached yet
            {
                TiltLeft();
            }
            
            else if (currentTilt > 0)       //if the world is rotated to the right and the button for left rotation is pressed, the world immediately rotates to default
            {
                TiltBack();
            }
            if (currentTilt == 0 || currentTilt < -maxTilt)  //stop tilting when the rotation is back to default or the maxTilt is reached 
            {
                tiltLeft = false;
            }
        }

        if (tiltTime > 1f)      //stops tilting when to targetRotation is reached
        {
            tiltRight = false;
            tiltLeft = false;
        }
    }

    //Author: Melanie Jäger
    //Method that is called when teh event is triggered
    private void TiltBackMechanic(float tiltDirection)
    { 
        //after tilting the world aotumatically tilts back to default position
        if (tiltDirection == 0)
        {
            isAxisInUse = false;
            tiltBackTime = 0;
            TiltBackDefault();
        }
    }


    //Author: Melanie Jäger
    //tilts world to the right
    private void TiltRight()
    {
        setWorldPosition();     //empty Gameobject gets the same position as the player so that the world can rotate around the player
        setWorldParent();       //grid and background are set as children from the empty gameobject worldRotation so that they can rotate with the empty gameobject

        tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRight, tiltTime);
        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetRight, tiltTime);
        currentTilt = transform.rotation.z;

        unsetWorldParent();     //unsets the parent for grid and background so that they act independently
    }

    //Author: Melanie Jäger
    //tilts world to the left
    private void TiltLeft()
    {
        setWorldPosition();     //empty Gameobject gets the same position as the player so that the world can rotate around the player
        setWorldParent();       //grid and background are set as children from the empty gameobject worldRotation so that they can rotate with the empty gameobject

        tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

        transform.rotation = Quaternion.Lerp(transform.rotation, targetLeft, tiltTime);
        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetLeft, tiltTime);
        currentTilt = transform.rotation.z;

        unsetWorldParent();     //unsets the parent for grid and background so that they act independently
    }

    //Author: Melanie Jäger
    //Tilts world back to deafultRotation when the opposite button is pressed
    private void TiltBack()
    {
        setWorldPosition();     //empty Gameobject gets the same position as the player so that the world can rotate around the player
        setWorldParent();       //grid and background are set as children from the empty gameobject worldRotation so that they can rotate with the empty gameobject

        tiltTime += tiltSpeed * timeController.getSpeedAdjustedDeltaTime();

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), tiltTime);
        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(0, 0, 0), tiltTime);
        currentTilt = transform.rotation.z;

        unsetWorldParent();     //unsets the parent for grid and background so that they act independently
    }


    //Author: Melanie Jäger
    //tilts the world back automatically when tilt was activated
    private void TiltBackDefault()
    {
        setWorldParent();

        tiltBackTime += tiltBackSpeed * timeController.getSpeedAdjustedDeltaTime();

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), tiltBackTime);
        //transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, 0, 0), tiltBackTime);
        currentTilt = transform.rotation.z;
        
        unsetWorldParent();
    }

    //Author: Melanie Jäger
    //sets the position for the rotation
    private void setWorldPosition()
    {
        transform.position = player.transform.position;
    }

    //Author: Melanie Jäger
    //sets worldRotation as parent to grid and background
    private void setWorldParent()
    {
        level.transform.SetParent(transform);
    }

    //Author: Melanie Jäger
    //unsets worldRotation as parent for grid and background
    private void unsetWorldParent()
    {
        level.transform.SetParent(null);
    }
}

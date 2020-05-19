using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActions;

public class LevelController : MonoBehaviour
{
    //Author: Melanie Jäger

    //public values can be changed in the editor
    
    public float tiltAngle = 11.25f;        //orientationvalue for incremental rotation, on avarage exact value is slightly bigger
    public float maxTilt = 0.35f;           //value für the number of Steps, 0.35 equals four steps in every direction with an angle of 11.25 degree
                                            //with 0.35 the angle is not going to be bigger than 45 degree
    public float tiltSpeed = 0.05f;         //speedvalue for the tilt
    public float tiltBackSpeed = 0.015f;    //speedvalue for the automatic backtilt

    protected GameObject player;            //player object for the rotation center
    protected float currentTilt;            //value that holds the current tilt
    protected GameObject grid;              //Gameobject that holds the grid, so that the grid can be rotated
    protected GameObject background;        //Gameobject that holds the backgroung, so that the background can be rotated

    private bool isAxisInUse = false;
    private float tiltTime;                 
    private float tiltBackTime;
    private bool tiltRight = false;
    private bool tiltLeft = false;

    private Quaternion targetRight;     
    private Quaternion targetLeft;

    //Author: Melanie Jäger
    void Start()
    {
        currentTilt = transform.rotation.z;   

        grid = GameObject.Find("Grid");                         //find grid in the scene
        background = GameObject.Find("Background");             //find background in the scene
        player = GameObject.Find("Player");                     //Gameobject for the player
                                                                //if these objects get different names these need to be changed
        targetRight = new Quaternion();
        targetLeft = new Quaternion();

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
            }
        }

        if (tiltRight == true)
        {
            
            if (currentTilt >= 0 && currentTilt < maxTilt)  //rotate the world to the right when the current rotation equals or is bigger than 0 and the limit to maxTilt is not reached yet
            {
                TiltRight(targetRight);
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
    public void TiltRight(Quaternion target)
    {
        setWorldPosition();     //empty Gameobject gets the same position as the player so that the world can rotate around the player
        setWorldParent();       //grid and background are set as children from the empty gameobject worldRotation so that they can rotate with the empty gameobject

        tiltTime += tiltSpeed;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRight, tiltTime);
        currentTilt = transform.rotation.z;  

        unsetWorldParent();     //unsets the parent for grid and background so that they act independently
    }

    //Author: Melanie Jäger
    //tilts world to the left
    public void TiltLeft()
    {
        setWorldPosition();     //empty Gameobject gets the same position as the player so that the world can rotate around the player
        setWorldParent();       //grid and background are set as children from the empty gameobject worldRotation so that they can rotate with the empty gameobject

        tiltTime += tiltSpeed;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetLeft, tiltTime);
        currentTilt = transform.rotation.z;

        unsetWorldParent();     //unsets the parent for grid and background so that they act independently
    }

    //Author: Melanie Jäger
    //Tilts world back to deafultRotation when the opposite button is pressed
    public void TiltBack()
    {
        setWorldPosition();     //empty Gameobject gets the same position as the player so that the world can rotate around the player
        setWorldParent();       //grid and background are set as children from the empty gameobject worldRotation so that they can rotate with the empty gameobject

        tiltTime += tiltSpeed;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), tiltTime);
        currentTilt = transform.rotation.z;

        unsetWorldParent();     //unsets the parent for grid and background so that they act independently
    }


    //Author: Melanie Jäger
    //tilts the world back automatically when tilt was activated
    public void TiltBackDefault()
    {
        setWorldParent();

        tiltBackTime += tiltBackSpeed;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), tiltBackTime);
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
        grid.transform.SetParent(transform);
        background.transform.SetParent(transform);
    }

    //Author: Melanie Jäger
    //unsets worldRotation as parent for grid and background
    private void unsetWorldParent()
    {
        grid.transform.SetParent(null);
        background.transform.SetParent(null);
    }
}

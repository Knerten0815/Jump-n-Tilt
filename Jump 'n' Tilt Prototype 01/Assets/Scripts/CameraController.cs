//Author: Kevin Zielke

using System.Collections;
using UnityEngine;
using GameActions;

//This class controls all camera movement, such as following the player through the level and shaking effects on certain events
public class CameraController : MonoBehaviour
{
    // variables for experimenting with the shake animation
    [SerializeField] float shakeDuration = 1.2f;        //how long the camera will shake
    [SerializeField] float shakeStartRate = 0.05f;      //how fast the camera will shake (lower is faster)
    [SerializeField] float shakeAmount = 0.2f;          //how heavy the camera will shake
    [SerializeField] float shakeRelease = 1.2f;         //how much the shakeRate slows down (should be >1. Speeds up the Shake Rate if <1)

    private Camera cam;                                 //The main camera
    private Transform parentTrans;                      //The parent of the main camera
    private Transform playerTrans;                      //The current position of the Player Character

    private Vector2 startLerpPos, endLerpPos;            //help variables for interpolating between shake positions
    private float lerpStartTime, currentShakeRate;

    // initializing the main camera and its parent, as well as the transform of the player character
    // also subscribing to events
    void Start()
    {
        cam = Camera.main;

        playerTrans = GameObject.Find("Player").transform;
        parentTrans = cam.transform.parent;

        //only for testing. Needs to be subscribed to the actual Tilt/Stomp event, not just the PlayerInput
        PlayerInput.onTiltDown += CameraShake;
        PlayerInput.onStomp += CameraShake;
    }

    private void Update()
    {
        //follow the player through the level
        parentTrans.position = followPlayer();

        //shake the camera
        if (startLerpPos != null && endLerpPos != null && lerpStartTime != 0)
            cam.transform.localPosition = smoothShake();        
    }

    //general CameraShake
    private void CameraShake()
    {
        StartCoroutine(DoShake(shakeDuration));        
    }

    ///does anybody know how to make this cleaner? PlayerInput.onTilt übergibt ein float Argument direction, was es auch muss um zu
    ///wissen in welche Richtung man tiltet, aber das ist unrelevant für CameraShake. Finde es irgendwie dumm eine extra-Überladung
    ///dafür schreiben zu müssen
    private void CameraShake(float notUsed)
    {
        CameraShake();
    }

    //iterates the CameraShake for duration seconds and resets the camera afterwards
    IEnumerator DoShake(float duration)
    {
        //start shaking
        Coroutine setShakePos = StartCoroutine(Shake());
        
        //wait for duration seconds and stop shake
        yield return new WaitForSeconds(duration);
        StopCoroutine(setShakePos);

        //reset camera
        endLerpPos = new Vector3(0, 0, -10);  //only works with static camera for now, will need followPlayer function to work properly
    }

    //sets random positions for camera shaking and slows down the shake rate over time
    IEnumerator Shake()
    {
        currentShakeRate = shakeStartRate;
        while (true)
        {
            Vector2 camPos = new Vector3(0, 0);

            //some random quotation for random position
            camPos.x += Random.value * shakeAmount * 2 - shakeAmount;
            camPos.y += Random.value * shakeAmount * 2 - shakeAmount;

            //set new camera position
            lerpStartTime = Time.time;
            endLerpPos = camPos;

            //wait and increase waittime for next iteration
            yield return new WaitForSeconds(currentShakeRate);
            startLerpPos = endLerpPos;
            currentShakeRate *= shakeRelease;
        }
    }

    //interpolates between the shake positions for smoother shaking
    private Vector3 smoothShake()
    {
        float t = ((Time.time - lerpStartTime) / currentShakeRate);
        Vector2 lerp = Vector2.Lerp(startLerpPos, endLerpPos, t);

        return new Vector3(lerp.x, lerp.y, 0);
    }

    private Vector3 followPlayer()
    {
        return new Vector3(playerTrans.position.x, playerTrans.position.y, -10);
    }

    //unsubscribing events
    private void OnDisable()
    {
        PlayerInput.onTiltDown -= CameraShake;
        PlayerInput.onStomp -= CameraShake;
    }
}

//Author: Kevin Zielke

using System.Collections;
using UnityEngine;
using GameActions;

//This class controls all camera movement, such as following the player through the level and shaking effects on certain events
public class CameraController : MonoBehaviour
{
    private Camera cam;                                 //The main camera

    [SerializeField] float shakeDuration = 1.2f;        //how long the camera will shake
    [SerializeField] float shakeRate = 0.05f;           //how fast the camera will shake (lower is faster)
    [SerializeField] float shakeAmount = 0.2f;          //how much the camera will shake
    [SerializeField] float shakeRelease = 1.2f;         //how much the shakeRate slows down

    //initializing main camera and subscribing to events
    void Start()
    {
        cam = Camera.main;

        //only for testing. Needs to be subscribed to the actual Tilt/Stomp event, not just the PlayerInput
        PlayerInput.onTilt += CameraShake;
        PlayerInput.onStomp += CameraShake;
    }

    //unsubscribing events
    private void OnDisable()
    {
        PlayerInput.onTilt -= CameraShake;
        PlayerInput.onStomp -= CameraShake;
    }

    //general CameraShake
    private void CameraShake()
    {
        StartCoroutine(DoShake(shakeDuration, shakeAmount, shakeRate, shakeRelease));
    }

    ///does anybody know how to make this cleaner? PlayerInput.onTilt übergibt ein float Argument direction, was es auch muss um zu
    ///wissen in welche Richtung man tiltet, aber das ist unrelevant für CameraShake. Finde es irgendwie dumm eine extra-Überladung
    ///dafür schreiben zu müssen
    private void CameraShake(float notUsed)
    {
        CameraShake();
    }

    //iterating the CameraShake for duration seconds and passing through amount, rate and release to the actual shake
    IEnumerator DoShake(float duration, float amount, float rate, float release)
    {
        //save camera position before shake
        Vector3 camReset = cam.transform.position;

        //start shaking
        Coroutine shake = StartCoroutine(Shake(amount, rate, release));
        
        //wait for duration seconds and stop shake
        yield return new WaitForSeconds(duration);
        StopCoroutine(shake);

        //reset camera
        cam.transform.position = camReset; //only works with static camera for now, will need followPlayer function to work properly
    }

    //shakes the camera
    IEnumerator Shake(float amount, float rate, float release)
    {
        while (true)
        {
            Vector3 camPos = cam.transform.position;

            float rmdX = Random.value;
            float rmdY = Random.value;
            //some random quotation for random position
            camPos.x += rmdX * amount * 2 - amount;
            camPos.y += rmdY * amount * 2 - amount;

            //set new camera position
            cam.transform.position = camPos;

            //wait for rate seconds and increase waittime
            yield return new WaitForSeconds(rate);
            rate *= release;            
        }
    }

    //shake needs to be smoothend. No idea how to do that right now.
    private Vector3 smoothShaking()
    {
        return new Vector3();
    }

}

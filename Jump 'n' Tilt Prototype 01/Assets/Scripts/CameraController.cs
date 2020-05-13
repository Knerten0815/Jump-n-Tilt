//Author: Kevin Zielke
//This class controls all camera movement, such as following the player
//through the level and shaking effects on certain events e.g. Tilting

using System.Collections;
using UnityEngine;
using GameActions;
using UnityEngine.XR.WSA.Input;

public class CameraController : MonoBehaviour
{
    private Camera cam;                         //The main camera

    //[SerializeField] float ;
    //[SerializeField] float ;
    [SerializeField] float shakeDuration = 1.2f, shakeRate = 0.05f, shakeAmount = 0.2f, shakeRelease = 1.2f;


    void Start()
    {
        cam = Camera.main;

        //only for testing. Needs to be subscribed to the actual Tilt/Stomp event, not just the PlayerInput
        PlayerInput.onTilt += TiltShake;
        PlayerInput.onStomp += CameraShake;
    }

    private void OnDisable()
    {
        PlayerInput.onTilt -= TiltShake;
        PlayerInput.onStomp -= CameraShake;
    }

    private void CameraShake()
    {
        StartCoroutine(DoShake(shakeDuration, shakeAmount, shakeRate, shakeRelease));
    }

    private void TiltShake(float direction)
    {
        StartCoroutine(DoShake(shakeDuration, shakeAmount * direction, shakeRate, shakeRelease));
    }
    IEnumerator DoShake(float duration, float amount, float rate, float release)
    {
        Vector3 camReset = cam.transform.position;
        Coroutine shake = StartCoroutine(Shake(amount, rate, release));
        yield return new WaitForSeconds(duration);
        StopCoroutine(shake);
        cam.transform.position = camReset; //only works with static camera for now, will need followPlayer function to work properly
    }

    IEnumerator Shake(float amount, float rate, float release)
    {
        while (true)
        {
            Vector3 camPos = cam.transform.position;

            float rmdX = Random.value;
            float rmdY = Random.value;
            camPos.x += rmdX * amount * 2 - amount;
            camPos.y += rmdY * amount * 2 - amount;

            cam.transform.position = camPos;

            yield return new WaitForSeconds(rate);

            rate *= release;            
        }
    }

    private Vector3 smoothShaking()
    {
        return new Vector3();
    }

}

//Author: Kevin Zielke

using System.Collections;
using UnityEngine;
using GameActions;
using UnityEngine.Tilemaps;

/// <summary>
/// This class controls all camera movement, such as following the player through the level and shaking effects on certain events
/// </summary>
public class CameraController : MonoBehaviour
{
    // variables for experimenting with the shake animation
    [SerializeField] float shakeDuration = 1.2f;        //how long the camera will shake
    [SerializeField] float shakeStartRate = 0.05f;      //how fast the camera will shake (lower is faster)
    [SerializeField] float shakeAmount = 0.2f;          //how heavy the camera will shake
    [SerializeField] float shakeRelease = 1.2f;         //how much the shakeRate slows down (should be >1. Speeds up the Shake Rate if <1)

    [SerializeField] float yOffset = 1f;                //offsets the camera in y-position
    [SerializeField] float xOffset = 4f;                //offsets the camera in x-position
    [SerializeField] float xSmoothTime = 0.4f;          //smooths the camera following
    [SerializeField] float ySmoothTime = 0.2f;          //smooths the camera following

    private Camera cam;                                 //The main camera
    private Transform parentTrans;                      //The parent of the main camera, that is following the player
    private GameObject player;                          //The current position of the Player Character
    private PlayerCharacter playerCtrl;                 //Player Controller for velocity
    private Vector2 offset;                             //current offset of the camera to the player
    public TilemapCollider2D lvlBounds;                //The TilemapCollider of the level: Needed for getting boundaries of the level

    ///Once the angle of the tilt is settled, the tilemap needs to be adjusted to show enough ground and walls, so that no empty background is seen.
    ///Once the tilemap is adjusted, these lvlBounds need to be scaled down accordingly, so that the camera won't clamp to the outer bounds of the
    ///tilemap, but some reasonable section of it.

    private Vector2 startLerpPos, endLerpPos;            //help variables for interpolating between shake positions
    private float lerpStartTime, currentShakeRate;

    private Vector3 velo = Vector3.zero;                //help variable for camera follow

    // initializing the main camera and its parent, as well as the transform of the player character
    // also subscribing to events
    void Start()
    {
        cam = Camera.main;
        offset.x = xOffset;
        offset.y = yOffset;

        player = GameObject.Find("Player");
        parentTrans = cam.transform.parent;
        playerCtrl = player.GetComponent<PlayerCharacter>();
        //lvlBounds = GameObject.Find("Bounds").GetComponent<TilemapCollider2D>();

        PlayerCharacter.onFishCausedEarthquakeStart += CameraShake; //Changed by Marvin Winkler
    }

    private void FixedUpdate()
    {
        //follow the player through the level
        if(player != null)
            parentTrans.position = followTarget(player);

        //shake the camera
        if (startLerpPos != null && endLerpPos != null && lerpStartTime != 0)
            cam.transform.localPosition = smoothShake();
    }

    //unsubscribing events
    private void OnDisable()
    {
        PlayerCharacter.onFishCausedEarthquakeStart -= CameraShake;
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
            camPos.x += UnityEngine.Random.value * shakeAmount * 2 - shakeAmount;
            camPos.y += UnityEngine.Random.value * shakeAmount * 2 - shakeAmount;

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

    //follow a target through the level
    private Vector3 followTarget(GameObject target)
    {
        Vector3 camPos = new Vector3();
        float yMin, yMax, xMin, xMax;

        //stay in level boundaries
        yMin = lvlBounds.bounds.min.y + cam.orthographicSize;
        yMax = lvlBounds.bounds.max.y - cam.orthographicSize;
        xMin = lvlBounds.bounds.min.x + (cam.aspect * cam.orthographicSize);
        xMax = lvlBounds.bounds.max.x - (cam.aspect * cam.orthographicSize);

        //calculate new camera positions
        camPos.y = Mathf.Clamp(target.transform.position.y + offset.y, yMin, yMax);
        camPos.z = -10;
        //xOffset depends on movement direction
        if (player.GetComponent<PlayerCharacter>().getVelocity().x < 0)
            camPos.x = Mathf.Clamp(target.transform.position.x - offset.x, xMin, xMax);
        else
            camPos.x = Mathf.Clamp(target.transform.position.x + offset.x, xMin, xMax);

        //smooth following, dependent on x and y velocity of the player
        if (Mathf.Abs(playerCtrl.getVelocity().y) > playerCtrl.jumpHeight)
        {
            if (Mathf.Abs(playerCtrl.getVelocity().y) > 23)
            {
                return Vector3.SmoothDamp(parentTrans.position, camPos, ref velo, 0.05f);
            }

            return Vector3.SmoothDamp(parentTrans.position, camPos, ref velo, ySmoothTime);
        }
        else
            return Vector3.SmoothDamp(parentTrans.position, camPos, ref velo, xSmoothTime);
    }
}

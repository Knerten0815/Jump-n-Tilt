//Author: Kevin Zielke

using System.Collections;
using TimeControlls;
using UnityEngine;
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

    [SerializeField] float minOffset = 0f;              //minimal camera offset in players move direction
    [SerializeField] float maxOffset = 4f;              //maximum offset in players move direction
    [SerializeField] float slideYOffset = 18f;          //offsets the camera in y-position while sliding down
    [SerializeField] float xSmoothTime = 0.4f;          //smooths the camera following

    private Camera cam;                                 //The main camera
    private Transform parentTrans;                      //The parent of the main camera, that is following the player
    private GameObject player;                          //The current position of the Player Character
    private PlayerCharacter playerCtrl;                 //Player Controller for velocity
    private Vector3 offset;                             //current offset of the camera to the player
    private TimeController timeCtrl;                    //Time Speed is used for fixing the camera offset during sliding
    public TilemapCollider2D lvlBounds;                 //The TilemapCollider of the level: Needed for getting boundaries of the level

    private Vector2 startLerpPos, endLerpPos;           //help variables for interpolating between shake positions
    private float lerpStartTime, currentShakeRate;

    private Vector3 velo = Vector3.zero;                //help variable for camera follow

    // initializing the main camera and its parent, as well as the transform of the player character
    // also subscribing to events
    void Start()
    {
        cam = Camera.main;
        offset = Vector2.zero;

        player = GameObject.Find("Player");
        timeCtrl = GameObject.Find("TimeController").GetComponent<TimeController>();
        parentTrans = cam.transform.parent;
        playerCtrl = player.GetComponent<PlayerCharacter>();

        PlayerCharacter.onFishCausedEarthquakeStart += CameraShake; //Changed by Marvin Winkler
    }

    private void FixedUpdate()
    {
        //follow the player through the level
        if(player != null)
            parentTrans.position = followPlayer();

        //shake the camera
        if (startLerpPos != null && endLerpPos != null && lerpStartTime != 0)
            cam.transform.localPosition = smoothShake();
    }

    //unsubscribing events
    private void OnDisable()
    {
        PlayerCharacter.onFishCausedEarthquakeStart -= CameraShake;
    }

    /// <summary>
    /// general CameraShake. Starts a Coroutine in which the camera positions are lerped.
    /// </summary>
    /// <param name="notUsed"> Needs a float argument to be properly subscribed to the Tilt-Event, but doesn't actually use it.</param> 
    private void CameraShake(float notUsed)
    {
        StartCoroutine(DoShake(shakeDuration));
    }
    /// <summary>
    /// iterates the CameraShake for duration seconds and resets the camera afterwards
    /// </summary>
    /// <param name="duration">The absolute time length of the camera shake</param>
    IEnumerator DoShake(float duration)
    {
        //start shaking
        Coroutine setShakePos = StartCoroutine(Shake());

        //wait for duration seconds and stop shake
        yield return new WaitForSeconds(duration);
        StopCoroutine(setShakePos);

        //reset camera
        endLerpPos = new Vector3(0, 0, -10);
    }

    /// <summary>
    /// sets random positions for camera shaking and slows down the shake rate over time
    /// </summary>
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

    /// <summary>
    /// Interpolates between the shake positions for smoother shaking
    /// </summary>
    private Vector3 smoothShake()
    {
        float t = ((Time.time - lerpStartTime) / currentShakeRate);
        Vector2 lerp = Vector2.Lerp(startLerpPos, endLerpPos, t);

        return new Vector3(lerp.x, lerp.y, 0);
    }
    
    /// <summary>
    /// follow the Player trough the level.
    /// </summary>
    /// <returns>Vector3 for Camera Position</returns>
    private Vector3 followPlayer()
    {
        Vector3 camPos = new Vector3();
        float yMin, yMax, xMin, xMax;

        //stay in level boundaries
        yMin = lvlBounds.bounds.min.y + cam.orthographicSize;
        yMax = lvlBounds.bounds.max.y - cam.orthographicSize;
        xMin = lvlBounds.bounds.min.x + (cam.aspect * cam.orthographicSize);
        xMax = lvlBounds.bounds.max.x - (cam.aspect * cam.orthographicSize);

        float clampedVelocity = Mathf.Clamp(playerCtrl.getVelocity().magnitude, minOffset, maxOffset);

        Debug.DrawRay(player.transform.position, playerCtrl.getVelocity());

        //apply extra y-Offset, if the player is sliding downwards
        if(playerCtrl.isSliding && playerCtrl.getVelocity().y < 0 && playerCtrl.getVelocity().y > -0.8f)
        {
            Vector2 slideDownOffset = new Vector2(playerCtrl.getVelocity().x * timeCtrl.getTimeSpeed(), playerCtrl.getVelocity().y * slideYOffset);
            offset = (Vector2)player.transform.position + slideDownOffset;
        }
        //follow the player
        else
            offset = (Vector2)player.transform.position + (playerCtrl.getVelocity().normalized * clampedVelocity);


        //calculate new camera positions
        camPos.y = Mathf.Clamp(offset.y, yMin, yMax);
        camPos.x = Mathf.Clamp(offset.x, xMin, xMax);
        camPos.z = -10;

        return Vector3.SmoothDamp(parentTrans.position, camPos, ref velo, xSmoothTime);
    }
}
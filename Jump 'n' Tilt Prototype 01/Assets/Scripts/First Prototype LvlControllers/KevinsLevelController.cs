using UnityEngine;
using GameActions;

//Author: Kevin Zielke
public class KevinsLevelController : MonoBehaviour
{
    public float tiltInDegrees = 15;            //how much the world tilts in degrees
    public float rotationSpeed = 10f;           //how fast the camera rotates back to normal after tilting the world
    public float gravity = 9.8f;                //amount of gravity: 9.8 is unitys default

    float tilt;                             //current rotation of the world

    // Start is called before the first frame update
    void Start()
    {
        tilt = 0;
        PlayerInput.onTiltDown += setTilt;
    }

    // Update is called once per frame
    void Update()
    {
        //Reset tilting
        if (!Mathf.Approximately(tilt, 0))
        {
            if (tilt > 1)
                tilt -= Time.deltaTime * rotationSpeed;
            else if (tilt < -1)
                tilt += Time.deltaTime * rotationSpeed;
            else
            {
                tilt = 0;
            }
        }

        //rotate Camera
        Camera.main.transform.rotation = Quaternion.Euler(0, 0, tilt);
        //calculate new Gravity-Vector
        float tiltRad = Mathf.Deg2Rad * tilt;
        Physics2D.gravity = new Vector2(Mathf.Sin(tiltRad) * gravity, -Mathf.Cos(tiltRad) * gravity);
    }

    void setTilt(float direction)
    {
        tilt = direction * tiltInDegrees;
        Debug.Log(tilt);
    }

    private void OnDisable()
    {
        PlayerInput.onTiltDown -= setTilt;
    }
}

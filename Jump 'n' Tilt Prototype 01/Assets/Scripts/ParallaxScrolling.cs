///Author: Taken from Youtube Channel Dani: https://www.youtube.com/channel/UCIabPXjvT5BVTxRDPCBBOOQ
///Author: Link to video: https://www.youtube.com/watch?v=zit45k6CUMk
///Author: Tweaks and summary by Kevin Zielke

using UnityEngine;

/// <summary>
/// This class controls parallax background scrolling. Backgroundlayers need to be added to the scene.
/// The backgroundlayers then need to be spread out, so that their width exceeds the doubled camera width and
/// the center layer lies in the center of the camera.
/// Each backgroundlayer needs to be assigned to the sorting layer 'Background' and a corresponding layer in its
/// Sprite Renderer Component.
/// </summary>

public class ParallaxScrolling : MonoBehaviour
{
    private float length, startpos;
    //changed name of original variable cam to cameraHolder, to better fit our project
    private Transform cameraHolder;
    public float parallaxEffect;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        //made the cameraHolder private, and assigned the parent, so no drag'n'drop is needed
        cameraHolder = transform.parent;
    }

    void Update()
    {
        float temp = (cameraHolder.position.x * (1 - parallaxEffect));
        float dist = (cameraHolder.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length)
            startpos += length;
        else if (temp < startpos - length)
            startpos -= length;
    }
}

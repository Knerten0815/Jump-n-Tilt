using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Author: Michelle Limbach
public class Titlescreen : MonoBehaviour
{
   

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}

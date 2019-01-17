using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Camera> cameras;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void choiceRole(string role)
    {
        disableAllCameras();
        if(role == "buteur")
        {
            cameras[0].enabled = true;
        } else if (role == "mur")
        {
            cameras[2].enabled = true;
        } else if (role == "gardien")
        {
            cameras[1].enabled = true;
        }
    }

    private void disableAllCameras()
    {
        foreach(Camera camera in cameras)
        {
            camera.enabled = false;
        }
    }
}

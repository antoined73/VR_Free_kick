using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Camera> cameras;

    private Role roleChoosen;
    private Replay[] objectsTransform;

    private void Awake()
    {
        //launchRecording();        
    }

    private void launchRecording()
    {
        objectsTransform = (Replay[])FindObjectsOfType(typeof(Replay));
        if (objectsTransform.Length > 0)
        {
            objectsTransform[0].startRecording();
            Debug.Log("start");
            Task.Delay(5000).ContinueWith((task) => {
                Debug.Log("stop");
                objectsTransform[0].stopRecording();
                objectsTransform[0].startPlayingBack();
            });
        }
    }

    public void choiceRole(Role role)
    {
        disableAllCameras();
        this.roleChoosen = role;
        switch (role)
        {
            case Role.Goal : 
                cameras[0].enabled = true;
                //Display.displays[1].Activate();
                return;
            case Role.Shooter : cameras[2].enabled = true; return;
            default : return;
        }
    }

    public Role getRoleChoosen()
    {
        return roleChoosen;
    }

    private void disableAllCameras()
    {
        foreach(Camera camera in cameras)
        {
            camera.enabled = false;
        }
    }
}

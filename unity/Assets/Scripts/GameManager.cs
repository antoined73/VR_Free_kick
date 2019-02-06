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
        objectsTransform = (Replay[])FindObjectsOfType(typeof(Replay));
    }

    private void launchRecording()
    {
        foreach(Replay element in objectsTransform)
        {
            element.startRecording();
        }
    }

    private void stopRecording()
    {
        foreach (Replay element in objectsTransform)
        {
            element.stopRecording();
        }
    }

    private void startPlayingBack()
    {
        foreach (Replay element in objectsTransform)
        {
            element.startPlayingBack();
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

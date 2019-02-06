using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Camera> cameras;

    private Role roleChoosen;
    private Replay[] objectsTransform;

    private AttackUIActions attackUI;
    private bool isPlayingBack = false;

    private Rigidbody ballRigidBody;
    private List<Collider> handsCollider = new List<Collider>();
    private GameObject[] hands;

    internal void Reset()
    {
        this.stopPlayingBack();
        this.isPlayingBack = false;
        ballRigidBody.isKinematic = false;
        foreach (Collider hand in handsCollider)
        {
            hand.enabled = true;
        }
    }

    private void Awake()
    {
        ballRigidBody = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();
        objectsTransform = (Replay[])FindObjectsOfType(typeof(Replay));
        attackUI = GameObject.FindObjectOfType<AttackUIActions>();
        hands = GameObject.FindGameObjectsWithTag("goalHand");
        foreach(GameObject hand in hands)
        {
            handsCollider.Add(hand.GetComponent<Collider>());
        }
    }

    public void launchRecording()
    {
        ballRigidBody.isKinematic = false;
        foreach (Collider hand in handsCollider)
        {
            hand.enabled = true;
        }

        foreach(Replay element in objectsTransform)
        {
            element.startRecording();
        }
    }

    public void stopRecording()
    {
        ballRigidBody.isKinematic = true;
        foreach (Collider hand in handsCollider)
        {
            hand.enabled = false;
        }
        foreach (Replay element in objectsTransform)
        {
            element.stopRecording();
        }
        attackUI.ShowRetryBtn();
        this.startPlayingBack();
    }

    public void startPlayingBack()
    {
        if(!this.isPlayingBack)
        {
            this.isPlayingBack = true;

            foreach (Replay element in objectsTransform)
            {
                element.startPlayingBack();
            }
        }
    }

    private void stopPlayingBack()
    {
        this.isPlayingBack = false;

        foreach (Replay element in objectsTransform)
        {
            element.stopPlayingBack();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replay : MonoBehaviour
{
    private new Transform transform;
    private bool isRecording = false;
    private bool isPlayingBack = false;
    private List<Vector3> transformPosition;
    private List<Quaternion> transformRotation;
    private int currentLoop = 100;

    private float playRate = 0.033f;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isRecording)
        {
            Debug.Log("record");
            transformPosition.Add(transform.position);
            transformRotation.Add(transform.rotation);
        }
    }

    public void startPlayingBack()
    {
        currentLoop = 100;
        isPlayingBack = true;
        InvokeRepeating("PlayingBackRepeat", 0, this.playRate);
    }

    private void PlayingBackRepeat()
    {
        if(currentLoop < transformPosition.Count - 1 && isPlayingBack)
        {
            Debug.Log("playBack");
            transform.position = transformPosition[currentLoop];
            transform.rotation = transformRotation[currentLoop++];
        } else if (isPlayingBack)
        {
            currentLoop = 100;
        } else
        {
            this.stopPlayingBack();
            CancelInvoke();
        }
    }

    public void startRecording()
    {
        transformPosition = new List<Vector3>();
        transformRotation = new List<Quaternion>();
        isRecording = true;
    }

    public void stopRecording()
    {
        isRecording = false;
    }

    public void stopPlayingBack()
    {
        isPlayingBack = false;
    }
}

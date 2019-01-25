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
    private int currentLoop = 0;

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
            transformPosition.Add(transform.position);
            transformRotation.Add(transform.rotation);
        } else if (isPlayingBack && currentLoop < transformPosition.Count - 1)
        {
            transform.position = transformPosition[currentLoop++];
            transform.rotation = transformRotation[currentLoop++];
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

    public void startPlayingBack()
    {
        currentLoop = 0;
        isPlayingBack = true;
    }

    public void stopPlayingBack()
    {
        isPlayingBack = false;

    }
}

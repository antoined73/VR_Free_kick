using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCamera : MonoBehaviour
{
    public GameObject ball;

    public Vector3 offset;

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the ball's, but offset by the calculated offset distance.
        transform.position = ball.transform.position + offset;
    }
}

using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootBalloon : MonoBehaviour
{
    private Rigidbody rb;
    private GameManager gameController;
    public Transform startPointTransform;

    private AudioSource ballSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballSource = GetComponent<AudioSource>();
    }

    public void Shoot(Vector2 target, float direction, float power)
    {
        if (ballSource) ballSource.Play(); // sound effect

        rb.AddForce(direction, power / 2, power, ForceMode.Impulse);
    }

    public void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPointTransform.position;
    }
}

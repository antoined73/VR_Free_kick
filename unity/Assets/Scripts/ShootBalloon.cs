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

    public Vector2 t = Vector2.zero;
    public float p = 10;
    public float d = 0;
    public float effectPower = 0.25f;
    private bool onFloor;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ballSource = GetComponent<AudioSource>();
    }

    public void Shoot(Vector2 target, float direction, float power)
    {
        if (ballSource) ballSource.Play(); // sound effect
        d = direction;
        p = power;
        t = target;
        // target.y = 0 && target.x = 0 --> Milieu du ballon
        // y = -1 = bas / y = 1 = haut
        // x = -1 = gauche / x = 1 = droite
        // power 50 to 0
        // direction -15 to 15
        rb.AddForce(d, -t.y*p, p, ForceMode.Impulse);

        rb.AddTorque(0, -t.x * effectPower * p / 4f, 0, ForceMode.Impulse);

        InvokeRepeating("AddEffect", 0.05f, 0.01f);
        Invoke("CancelEffectIfAlreadyGrounded", 0.5f);
    }

    public void CancelEffectIfAlreadyGrounded()
    {
        if(onFloor) CancelInvoke();
    }

    public void AddEffect()
    {
        rb.AddForce(-t.x * effectPower * p, 0, 0, ForceMode.Force);
        rb.AddTorque(0, -t.x * effectPower * p/8f, 0, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CancelInvoke();
        Debug.Log("ignore");
    }

    private void OnCollisionExit(Collision collision)
    {
        onFloor = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        onFloor = true;
    }

    public void ResetBall()
    {
        CancelInvoke();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPointTransform.position;
    }
}

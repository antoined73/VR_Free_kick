using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootBalloon : ShootBalloonBehavior
{
    public float thrust;
    public float shootDirection;
    public float shootPower;
    public Vector2 target;
    public bool targetSettled;

    public List<Camera> cameras;

    private Rigidbody rb;
    private GameManager gameController;
    public Transform startPointTransform;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        this.targetSettled = false;
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && !targetSettled && gameController.getRoleChoosen() == Role.Shooter)
            {
                this.target = touch.position;
                cameras[1].enabled = false;
                cameras[0].enabled = true;
                Console.WriteLine(this.target.x + " ; " + this.target.y);
            }
        }
        if(Input.GetMouseButtonDown(0) && !targetSettled && gameController.getRoleChoosen() == Role.Shooter)
        {
            this.target = Input.mousePosition;
            cameras[1].enabled = false;
            cameras[0].enabled = true;
            Debug.Log(this.target.x + " ; " + this.target.y);
            this.targetSettled = true;
        }
    }

    public void TryShoot()
    {
        if (gameController.getRoleChoosen() == Role.Shooter)
        {
            if (networkObject != null) // connected
            {
                networkObject.SendRpc(RPC_SHOOT, Receivers.All, this.shootDirection, this.shootPower, this.target);
            }
            else // not connected
            {
                Shoot();
            }
        }
    }

    public void TryResetBall()
    {
        if (gameController.getRoleChoosen() == Role.Shooter)
        {
            if (networkObject != null) // connected
            {
                networkObject.SendRpc(RPC_RETRY, Receivers.All);
            }
            else // not connected
            {
                ResetBall();
            }
        }
    }

    public void Shoot()
    {
        rb.AddForce(this.shootDirection, this.shootPower/2, this.shootPower, ForceMode.Impulse);
    }

    public void directionValueUpdate(float newValue)
    {
        this.shootDirection = newValue;
    }

    public void powerValueUpdate(float newValue)
    {
        this.shootPower = newValue;
    }

    public void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPointTransform.position;
    }

    //RPC METHODS
    public override void Shoot(RpcArgs args)
    {
        this.shootDirection = args.GetNext<float>();
        this.shootPower = args.GetNext<float>();
        this.target = args.GetNext<Vector2>();
        Shoot();
    }

    public override void Retry(RpcArgs args)
    {
        ResetBall();
    }
}

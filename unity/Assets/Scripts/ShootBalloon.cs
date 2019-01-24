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
    public bool targetSetted = false;

    public List<Camera> cameras;

    private Rigidbody rb;
    private GameManager gameController;
   

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && !targetSetted)
            {
                this.target = touch.position;
                cameras[1].enabled = false;
                cameras[0].enabled = true;
                Console.WriteLine(this.target.x + " ; " + this.target.y);
            }
        }
    }

    public void TryShoot()
    {
        if (gameController.getRoleChoosen() == Role.Shooter)
        {
            if (networkObject != null) // connected
            {
                networkObject.SendRpc(RPC_SHOOT, Receivers.All);
            }
            else // not connected
            {
                Shoot();
            }
        }
    }

    public override void Shoot_RPC(RpcArgs args)
    {
        Shoot();
    }

    public void Shoot()
    {
        rb.AddForce(this.shootDirection, this.shootPower/2, this.shootPower, ForceMode.Impulse);
    }

    public void OnClick()
    {
        networkObject.SendRpc(RPC_SHOOT, Receivers.All);
    }

    public void directionValueUpdate(float newValue)
    {
        this.shootDirection = newValue;
    }

    public void powerValueUpdate(float newValue)
    {
        this.shootPower = newValue;
    }
}

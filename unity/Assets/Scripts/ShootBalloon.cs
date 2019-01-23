using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class ShootBalloon : ShootBalloonBehavior
{
    public float thrust;

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
            if (touch.phase == TouchPhase.Began)
            {
                TryShoot();
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
        rb.AddForce(0, thrust, thrust, ForceMode.Impulse);
    }

    public void OnClick()
    {
        networkObject.SendRpc(RPC_SHOOT, Receivers.All);
    }
}

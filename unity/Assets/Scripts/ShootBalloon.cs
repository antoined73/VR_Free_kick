using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class ShootBalloon : ShootBalloonBehavior
{
    public float thrust;
    public Rigidbody rb;
    private GameManager gameController;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && gameController.getRoleChoosen() == "buteur")
            {
                networkObject.SendRpc(RPC_SHOOT, Receivers.All);
            }
        }
    }

    public override void Shoot(RpcArgs args)
    {
        rb.AddForce(0, thrust, thrust, ForceMode.Impulse);
    }
}

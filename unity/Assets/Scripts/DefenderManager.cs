using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;

public class DefenderManager : DefenderManagerBehavior
{
    public GameObject defender;

    private DefenderBehavior[] defenders = new DefenderBehavior[0];

    // Call the RPC Jump method on the Server.
    public void JumpCall()
    {
        networkObject.SendRpc(RPC_JUMP, Receivers.Server);
    }

    public void GenerateCall()
    {
        networkObject.SendRpc(RPC_GENERATE_DEFENDERS, Receivers.Server, Random.Range(1, 5));
    }

    // Make the defenders jump on the server side.
    public override void Jump(RpcArgs args)
    {
        foreach (DefenderBehavior d in defenders)
        {
            d.gameObject.GetComponent<DefenderMovement>().Jump();
        }
    }

    public override void GenerateDefenders(RpcArgs args)
    {
        RemoveDefenders();
        int quantity = args.GetNext<int>();
        defenders = new DefenderBehavior[quantity];
        for (int x = 0; x < quantity; x++)
        {
            float position = x - 2.5f;
            // Instantiate a new Defender Network Object.
            DefenderBehavior def = NetworkManager.Instance.InstantiateDefender(0, new Vector3(transform.position.x + position, 1.5f, 0), Quaternion.identity);
            defenders[x] = def;
        }
    }

    public void RemoveDefenders()
    {
        foreach (DefenderBehavior d in defenders)
        {
            d.networkObject.Destroy();
        }
    }
}

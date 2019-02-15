using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayerMovement : ShooterPlayerMovementBehavior
{
    private void Update()
    {
        if (networkObject != null)
        {
            if (!networkObject.IsServer)
            {
                transform.position = networkObject.position;
            }
            else
            {
                networkObject.position = transform.position;
            }
        }
    }
}

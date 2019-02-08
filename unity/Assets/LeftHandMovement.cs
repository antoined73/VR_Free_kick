using BeardedManStudios.Forge.Networking.Generated;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandMovement : LeftGoalHandBehavior
{
    private void Update()
    {
        if (networkObject != null)
        {
            if (!networkObject.IsServer)
            {
                transform.position = networkObject.position;
                transform.rotation = networkObject.rotation;
            }
            else
            {
                networkObject.position = transform.position;
                networkObject.rotation = transform.rotation;
            }
        }
    }
}

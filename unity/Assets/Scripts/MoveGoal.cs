using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Generated;

public class MoveGoal : MoveGoalBehavior
{
    public float speed = 5.0f;
    public Camera goalCamera;

    private void Update()
    {
        if (networkObject != null)
        {
            if (!networkObject.IsOwner) // Is not owner of object
            {
                ClientUpdate();
            }
            else // Is owner of object
            {
                OwnerUpdate();
            }
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 movement = goalCamera.transform.right * Input.GetAxis("Horizontal")
            + goalCamera.transform.forward * Input.GetAxis("Vertical");
        transform.position += movement.normalized * speed * Time.deltaTime;
    }

    private void ClientUpdate()
    {
        transform.position = networkObject.position;
    }

    private void OwnerUpdate()
    {
        Move();
        SendPosition();
    }

    private void SendPosition()
    {
        if (networkObject != null) networkObject.position = transform.position;
    }
}

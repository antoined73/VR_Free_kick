using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class DefenderMovement : DefenderBehavior
{

    public Rigidbody rb;
    public float jumpForce = 200f;

    // Update is called once per frame
    private void Update()
    {
        if (networkObject != null)
        {
            if (!networkObject.IsServer)
            {
                transform.position = networkObject.position;
            }
            networkObject.position = transform.position;
        }
        rb.AddForce(Vector3.down * Time.deltaTime);
    }
    
    public void Jump()
    {
        rb.AddForce(new Vector3(0, jumpForce * Time.deltaTime, 0), ForceMode.Impulse);
    }
}

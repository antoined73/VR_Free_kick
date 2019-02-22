using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public class DefenderMovement : DefenderBehavior
{

    public Rigidbody rb;
    public float jumpForce = 100f;
    public bool isGrounded = true;

    public void Awake()
    {
        transform.parent = GameObject.FindGameObjectWithTag("DefenderWall").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        if (networkObject != null)
        {
            if (!networkObject.IsServer)
            {
                transform.position = networkObject.position;
            } else
            {
                networkObject.position = transform.position;
            }
        }
        rb.AddForce(Vector3.down * Time.deltaTime);
    }
    
    public void Jump()
    {
        if (isGrounded)
            rb.AddForce(new Vector3(0, jumpForce * Time.deltaTime * 1.5f, 0), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Stade")
        {
            Debug.Log("Collision with stadium detected");
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.name == "Stade")
        {
            Debug.Log("Collision exit with stadium detected");
            isGrounded = false;
        }
    }
}

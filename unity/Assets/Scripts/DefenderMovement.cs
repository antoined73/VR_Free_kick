using UnityEngine;

public class DefenderMovement : MonoBehaviour
{

    public Rigidbody rb;
    public float jumpForce = 200f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, jumpForce * Time.deltaTime, 0), ForceMode.Impulse);
        }
        rb.AddForce(Vector3.down * Time.deltaTime);
    }
}

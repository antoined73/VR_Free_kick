using UnityEngine;

public class DefenderCollision : MonoBehaviour
{

    private void Start()
    {
        // We make the base collider ignore the ball
        Physics.IgnoreCollision(GetComponentInChildren<BoxCollider>(), GameObject.FindGameObjectWithTag("Ball").GetComponent<Collider>());

        Collider[] colliders = GetComponentsInChildren<Collider>();
        // We make colliders inside the defender ignore each other
        foreach (Collider collider in colliders)
        {

            Collider[] otherColliders = GetComponentsInChildren<Collider>();
            foreach (Collider otherColider in otherColliders)
            {

                if (collider != otherColider)
                {
                    Physics.IgnoreCollision(collider, otherColider);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected : " + collision.collider.name);
        if (collision.collider.name == "Plane")
        {
            Debug.Log("Collision with stadium detected");
            GetComponentInParent<DefenderMovement>().isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Collision exit detected : " + collision.collider.name);
        if (collision.collider.name == "Plane")
        {
            Debug.Log("Collision exit with stadium detected");
            GetComponentInParent<DefenderMovement>().isGrounded = false;
        }
    }
}

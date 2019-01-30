using UnityEngine;

public class BuildWall : MonoBehaviour
{

    public Transform defender;

    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < 5; x++)
        {
            float position = x - 2.5f;
            Instantiate(defender, new Vector3(transform.position.x + position, 0, 0), Quaternion.identity);
        }
    }
}

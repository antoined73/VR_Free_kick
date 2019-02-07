using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float randomX = Random.Range(-10, 10);
        float randomZ = Random.Range(0, -20);
        /* string[] tags = { "MainCamera", "PointShootCamera", "ShooterModel", "StartPointBall", "Ball" };
         for(int i = 0; i < tags.Length; i++)
         {
             GameObject gameObj = GameObject.FindGameObjectWithTag(tags[i]);
             gameObj.transform.position = MoveAtRandomPosition(randomX, randomZ, gameObj.transform);
         }*/

        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera.transform.position = MoveAtRandomPosition(randomX, randomZ, mainCamera.transform);

        GameObject pointShootCamera = GameObject.FindGameObjectWithTag("PointShootCamera");
        pointShootCamera.transform.position = MoveAtRandomPosition(randomX, randomZ, pointShootCamera.transform);

        GameObject shooterModel = GameObject.FindGameObjectWithTag("ShooterModel");
        shooterModel.transform.position = MoveAtRandomPosition(randomX, randomZ, shooterModel.transform);

        GameObject startPointBall = GameObject.FindGameObjectWithTag("StartPointBall");
        startPointBall.transform.position = MoveAtRandomPosition(randomX, randomZ, startPointBall.transform);

        GameObject ball = GameObject.FindGameObjectWithTag("Ball");
        ball.transform.position = MoveAtRandomPosition(randomX, randomZ, ball.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 MoveAtRandomPosition(float randomX, float randomZ, Transform transform)
    {
        return new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }
}

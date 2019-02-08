using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    Vector3 initialShooterPosition;
    Vector3 initialStartPointPosition;
    Vector3 initialballPosition;
    Vector3 initialWallPosition;

    GameObject shooterPlayer;
    GameObject startPointBall;
    GameObject ball;
    GameObject defenderWall;

    private void Awake()
    {
        shooterPlayer = GameObject.FindGameObjectWithTag("ShooterPlayer");
        startPointBall = GameObject.FindGameObjectWithTag("StartPointBall");
        ball = GameObject.FindGameObjectWithTag("Ball");
        defenderWall = GameObject.FindGameObjectWithTag("DefenderWall");
        SavePositions();
    }

    public void GenerateRandomShootPosition()
    {
        float randomX = Random.Range(-8, 8);
        float randomZ = Random.Range(0, -10);

        shooterPlayer.transform.position = MoveAtRandomPosition(randomX, randomZ, initialShooterPosition);
        startPointBall.transform.position = MoveAtRandomPosition(randomX, randomZ, initialStartPointPosition);
        ball.transform.position = MoveAtRandomPosition(randomX, randomZ, initialballPosition);
        defenderWall.transform.position = MoveAtRandomPosition(randomX, randomZ, initialWallPosition);
    }

    private void SavePositions()
    {
        initialShooterPosition = shooterPlayer.transform.position;
        initialStartPointPosition = startPointBall.transform.position;
        initialballPosition = ball.transform.position;
        initialWallPosition = defenderWall.transform.position;
    }

    private Vector3 MoveAtRandomPosition(float randomX, float randomZ, Vector3 initPosition)
    {
        return new Vector3(initPosition.x + randomX, initPosition.y, initPosition.z + randomZ);
    }
}

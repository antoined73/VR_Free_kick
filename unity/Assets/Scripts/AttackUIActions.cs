using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUIActions : MonoBehaviour
{
    ShootBalloon shootScript;

    void Awake()
    {
        shootScript = GameObject.FindObjectOfType<ShootBalloon>();
    }

    public void LaunchShoot()
    {
        if (shootScript) shootScript.TryShoot();
        else Debug.LogError("ShootBalloon script not found in scene");
    }

    public void LaunchRetry()
    {
        if (shootScript) shootScript.TryResetBall();
        else Debug.LogError("ShootBalloon script not found in scene");
    }
}

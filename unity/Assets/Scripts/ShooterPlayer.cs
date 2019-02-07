using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterPlayer : ShootBalloonBehavior
{
    private GameManager gameController;
    private ShootBalloon shootBall;
    private BallDetector ballDetector;
    private AttackUIActions attackUI;
    private SpawnManager spawnManager;

    private AudioSource whistleSource;

    // Cameras used during shoot
    public Camera choiceTargetCamera;
    public Camera shootCamera;

    // flags for shooting
    public bool targetSettled;
    public bool ballShot;

    // Shoot parameters
    private float shootDirection = 0;
    private float shootPower = 30;
    private Vector2 shootTarget;
    private bool shootOrdered;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        shootBall = GameObject.FindObjectOfType<ShootBalloon>();
        ballDetector = GameObject.FindObjectOfType<BallDetector>();
        attackUI = GameObject.FindObjectOfType<AttackUIActions>();
        spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        spawnManager.GenerateRandomShootPosition();
        whistleSource = GetComponent<AudioSource>();
    }

    void Update()
    {
#if UNITY_ANDROID
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && !targetSettled && gameController.getRoleChoosen() == Role.Shooter)
            {
                RaycastHit hit;
                Ray ray = choiceTargetCamera.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform objectHit = hit.transform;
                    if (objectHit.tag.Equals("Ball"))
                    {
                        float x = Mathf.Clamp((hit.point.x - objectHit.position.x) * 11,-1,1);
                        float y = Mathf.Clamp((hit.point.y - objectHit.position.y) * 11,-1,1);
                    
                        Debug.Log(x+":"+y);
                        Vector2 target = new Vector2(x, y);
                        this.SetTarget(target);
                    }
                }
            }
        }
#else
        if (Input.GetMouseButtonDown(0) && !targetSettled && gameController.getRoleChoosen() == Role.Shooter)
        {
            RaycastHit hit;
            Ray ray = choiceTargetCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.tag.Equals("Ball"))
                {
                    float x = Mathf.Clamp((hit.point.x - objectHit.position.x) * 11,-1,1);
                    float y = Mathf.Clamp((hit.point.y - objectHit.position.y) * 11,-1,1);
                    
                    Debug.Log(x+":"+y);
                    Vector2 target = new Vector2(x, y);
                    this.SetTarget(target);
                }
            }
        }
#endif
    }

    //Get the screen size of an object in pixels, given its distance and diameter.
    float DistanceAndDiameterToPixelSize(float distance, float diameter)
    {

        float pixelSize = (diameter * Mathf.Rad2Deg * Screen.height) / (distance * choiceTargetCamera.fieldOfView);
        return pixelSize;
    }

    private void SetTarget(Vector2 position)
    {
        this.shootTarget = position;
        this.targetSettled = true;
        Debug.Log(this.shootTarget.x + " ; " + this.shootTarget.y);

        choiceTargetCamera.enabled = false;
        shootCamera.enabled = true;
        attackUI.ShowShootUI();
    }

    private void LaunchShootOrder()
    {
        this.gameController.launchRecording();
        shootOrdered = true;
        StartCoroutine(ShootOrder());
    }

    IEnumerator ShootOrder()
    {
        whistleSource.Play();
        yield return new WaitForSeconds(3);
        ballShot = true;
        shootBall.Shoot(this.shootTarget, this.shootDirection, this.shootPower);
        yield return new WaitForSeconds(6);
        this.gameController.stopRecording();
    }

    public bool TryLaunchShoot()
    {
        if (CanLaunchShoot())
        {
            if (networkObject != null) // connected
            {
                networkObject.SendRpc(RPC_SHOOT, Receivers.All, this.shootDirection, this.shootPower, this.shootTarget);
                return true;
            }
            else // not connected
            {
                LaunchShootOrder();
                return true;
            }
        }
        return false;
    }

    public bool TryResetBall()
    {
        if (CanRetryShoot())
        {
            if (networkObject != null) // connected
            {
                networkObject.SendRpc(RPC_RETRY, Receivers.All);
                return true;
            }
            else // not connected
            {
                LauchRetryOrder();
                return true;
            }
        }
        return false;
    }

    private void LauchRetryOrder()
    {
        ballDetector.Reset();
        gameController.Reset();
        targetSettled = false;
        shootOrdered = false;
        ballShot = false;
        choiceTargetCamera.enabled = true;
        shootCamera.enabled = false;
        shootBall.ResetBall();
        spawnManager.GenerateRandomShootPosition();
    }

    private bool CanLaunchShoot()
    {
        return shootBall && gameController.getRoleChoosen() == Role.Shooter && (!shootOrdered && !ballShot && targetSettled);
    }

    private bool CanRetryShoot()
    {
        return shootBall && gameController.getRoleChoosen() == Role.Shooter && (shootOrdered && ballShot && targetSettled);
    }

    internal void SetPowerValue(float newValue)
    {
        this.shootPower = newValue;
    }

    internal void SetDirectionValue(float newValue)
    {
        this.shootDirection = newValue;
    }

    //RPC METHODS
    public override void Shoot(RpcArgs args)
    {
        this.shootDirection = args.GetNext<float>();
        this.shootPower = args.GetNext<float>();
        this.shootTarget = args.GetNext<Vector2>();
        LaunchShootOrder();
    }

    public override void Retry(RpcArgs args)
    {
        LauchRetryOrder();
    }
}

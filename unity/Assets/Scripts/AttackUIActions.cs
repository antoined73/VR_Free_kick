using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUIActions : MonoBehaviour
{
    ShooterPlayer shootPlayer;

    public CanvasGroup shootBtn;
    public CanvasGroup retryBtn;
    public CanvasGroup slidersUI;

    void Awake()
    {
        shootPlayer = GameObject.FindObjectOfType<ShooterPlayer>();
        ActivateUI(shootBtn, false);
        ActivateUI(slidersUI, false);
        ActivateUI(retryBtn, false);
    }

    private void ActivateUI(CanvasGroup group, bool activate)
    {
        group.alpha = (activate ? 1 : 0);
        group.interactable = activate;
        group.blocksRaycasts = activate;
    }

    public void LaunchShoot()
    {
        if (shootPlayer)
        {
            if (shootPlayer.TryLaunchShoot())
            {
                ActivateUI(shootBtn, false);
                ActivateUI(slidersUI, false);
            }
        }
        else Debug.LogError("ShootPlayer script not found in scene");
    }

    public void LaunchRetry()
    {
        if (shootPlayer) {
            if (shootPlayer.TryResetBall())
            {
                ActivateUI(retryBtn, false);
            }
        }
        else Debug.LogError("ShootPlayer script not found in scene");
    }

    public void directionValueUpdate(float newValue)
    {
        if (shootPlayer) shootPlayer.SetDirectionValue(newValue);
    }

    public void powerValueUpdate(float newValue)
    {
        if (shootPlayer) shootPlayer.SetPowerValue(newValue);
    }

    internal void ShowShootUI()
    {
        ActivateUI(shootBtn, true);
        ActivateUI(slidersUI, true);
    }

    internal void ShowRetryBtn()
    {
        ActivateUI(retryBtn, true);
    }
}

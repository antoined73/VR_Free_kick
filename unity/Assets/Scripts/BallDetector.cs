using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDetector : MonoBehaviour
{
    private const string BALL_TAG = "Ball";
    private bool containBall = false;
    private bool ballDetectedOnce = false;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(BALL_TAG) && !ballDetectedOnce)
        {
            ballDetectedOnce = true;
            SetContainsBall(true);
            audioManager.PlayGoalSound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(BALL_TAG))
        {
            SetContainsBall(false);
        }
    }

    private void SetContainsBall(bool newVal)
    {
        this.containBall = newVal;
    }

    internal void Reset()
    {
        this.containBall = false;
        this.ballDetectedOnce = false;
    }
}

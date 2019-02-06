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
    private GameManager gameManager;

    private void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(BALL_TAG) && !ballDetectedOnce)
        {
            StartCoroutine(EnteredField());
        }
    }

    IEnumerator EnteredField()
    {
        ballDetectedOnce = true;
        SetContainsBall(true);
        audioManager.PlayGoalSound();
        yield return new WaitForSeconds(3);
        this.gameManager.stopRecording();
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

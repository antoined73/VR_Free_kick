using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> goalClips;

    public AudioSource goalSource;
    public AudioSource ambianceSource;
    public AudioSource musicSource;

    void Start()
    {
        ambianceSource.Play();
        musicSource.Play();
    }

    void Update()
    {
        
    }

    internal void PlayGoalSound()
    {
        goalSource.clip = goalClips[0];
        goalSource.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    [Header("Rythm Management")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int tempo;
    [SerializeField] private RhythmEvent[] rhythmEvents;

    private float animTime;
    private float frequency;
    private int lastTrackTime = 0;


    // Start is called before the first frame update
    void Start()
    {
        animTime = 60f / tempo;
        frequency = audioSource.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        int trackTime = (int)(audioSource.timeSamples / (frequency));
        //UnityEngine.Debug.Log(trackTime);
        if (trackTime == 0 && trackTime != lastTrackTime)
        {
            //change speed
        }

        if (trackTime != lastTrackTime)
        {
            lastTrackTime = trackTime;
        }

        //send signals to rhythm based events
        foreach (RhythmEvent rhythmEvent in rhythmEvents)
        {
            float sampledTime = audioSource.timeSamples / (frequency * rhythmEvent.GetIntervalLength(tempo));
            //UnityEngine.Debug.Log(sampledTime);
            rhythmEvent.CheckForNewInterval(sampledTime);
        }
    }

    public float GetAnimTime()
    {
        return animTime;
    }
}

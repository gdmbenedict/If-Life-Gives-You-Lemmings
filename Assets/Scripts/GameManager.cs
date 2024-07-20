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


    // Start is called before the first frame update
    void Start()
    {
        animTime = 60f / tempo;
        frequency = audioSource.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
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

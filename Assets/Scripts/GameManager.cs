using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Management")]
    [SerializeField] private TerrainManager terrainManager;

    [Header("Rythm Management")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int tempo;
    [SerializeField] private RhythmEvent[] rhythmEvents;
    private float secPerBeat;
    private float frequency;


    // Start is called before the first frame update
    void Start()
    {
        secPerBeat = 60f / tempo;
        terrainManager.SetMoveTime(secPerBeat);
        frequency = audioSource.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        //send signals to rhythm based events
        foreach (RhythmEvent rhythmEvent in rhythmEvents)
        {
            float sampledTime = audioSource.timeSamples / (frequency * rhythmEvent.GetIntervalLength(tempo));
            UnityEngine.Debug.Log(sampledTime);
            rhythmEvent.CheckForNewInterval(sampledTime);
        }
    }
}

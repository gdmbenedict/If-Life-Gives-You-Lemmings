using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //connections
    private PlayerController playerController;
    private ProgramManager programManager;

    [Header("Rythm Management")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private int tempo;
    [SerializeField] private float speedIncrement = 0.1f;
    [SerializeField] private RhythmEvent[] rhythmEvents;

    private float speed = 1f;
    private float mixerPitch = 100f;
    private float animTime;
    private float frequency;
    private int lastTrackTime = 0;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        programManager = FindObjectOfType<ProgramManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CalcAnimTime();
        frequency = audioSource.clip.frequency;
        StartCoroutine(programManager.StartSequence(gameObject.GetComponent<GameManager>()));
    }

    // Update is called once per frame
    void Update()
    {
        int trackTime = (int)(audioSource.timeSamples / (frequency));

        //checking for track reset
        //UnityEngine.Debug.Log(trackTime);
        if (trackTime == 0 && trackTime != lastTrackTime)
        {
            //changing speed
            speed += speedIncrement; //change recoreded speed (used in other scripts)
            mixerPitch -= speedIncrement/2; //change mixer pitch
            audioSource.pitch = speed; //set new source pitch
            audioMixer.SetFloat("MixerLevelPitch", mixerPitch); //set new mixer pitchs

            //update speeds
            CalcAnimTime();
            playerController.calcTimings(speed);

        }

        audioMixer.GetFloat("MixerLevelPitch", out mixerPitch);
        //UnityEngine.Debug.Log(mixerPitch);

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

    public void StartGame()
    {
        audioSource.Play();
    }

    public float GetAnimTime()
    {
        return animTime;
    }

    public void CalcAnimTime()
    {
        animTime = (60f / tempo)/speed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            audioSource.UnPause();
        }
        else
        {
            Time.timeScale = 0;
            audioSource.Pause();
        }
    }
}

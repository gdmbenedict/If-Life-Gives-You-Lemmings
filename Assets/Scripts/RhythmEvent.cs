using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class RhythmEvent
{
    [SerializeField] private int every_X_beats;
    [SerializeField] private UnityEvent triggers;
    private int lastInterval;

    /// <summary>
    /// Accessor method that returns the length in seconds in between triggers of event
    /// </summary>
    /// <param name="bpm">the beats per minute of the song the interval is tied to</param>
    /// <returns>length in seconds in between triggers of event</returns>
    public float GetIntervalLength(int bpm)
    {
        return 60f / (bpm * (1f / every_X_beats));
    }

    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            triggers.Invoke();
        }
    }
}

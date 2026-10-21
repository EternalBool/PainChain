using System;
using UnityEngine;
using UnityEngine.Events;

public class BopManager : MonoBehaviour
{
    [SerializeField] private float bpm;
    [SerializeField] private int beatsPerBar = 4;
    [SerializeField] private int signatureBeat = 4;
    [SerializeField] private AudioSource auso;
    [SerializeField] private Intervals[] intervals;

    public float secondsPerBeat => (60f/bpm) * (4f/signatureBeat);
    public float secondsPerBar => secondsPerBeat * beatsPerBar;

    private void Update()
    {
        if (auso == null || auso.clip == null) return;
        float seconds = auso.timeSamples / (float)auso.clip.frequency;
        foreach (Intervals interval in intervals)
        {
            float sampleTime = seconds / interval.GetIntervalLength(secondsPerBar);
            interval.CheckForInterval(sampleTime);
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float triggersPerBar = 1f;
    [SerializeField] private UnityEvent _trigger;
    private int lastInterval = -1;

    public float GetIntervalLength(float secondsPerBar)
    {
        return secondsPerBar / triggersPerBar;
    }
    public void CheckForInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != lastInterval)
        {
            lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}
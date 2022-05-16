using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TimeValue
{
    public int hours;
    [Range(0, 60)] public int minutes;
    [Range(0, 60)] public float seconds;

    /// <summary>
    /// The total time in seconds, used for calculations.
    /// </summary>
    public float InSeconds => (hours * 3600) + (minutes * 60) + seconds;
    public override string ToString() => ToString(0);
    public string ToString(int decimalPlaces)
    {
        return hours + ":" + minutes + ":" + MiscMath.RoundToDecimalPlaces(seconds, decimalPlaces);
    }

    public TimeValue(int h, int m, float s)
    {
        hours = h;
        minutes = m;
        seconds = s;
    }
    public TimeValue(float totalTimeInSeconds)
    {
        hours = Mathf.FloorToInt(totalTimeInSeconds / 3600);
        totalTimeInSeconds -= hours * 3600;
        minutes = Mathf.FloorToInt(totalTimeInSeconds / 60);
        totalTimeInSeconds -= minutes * 60;
        seconds = totalTimeInSeconds;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputAxis
{
    Horizontal,
    Vertical,
    Both
}

public static class TouchFunction
{
    public static Vector2 LimitProcessedInput(Vector2 initialInput, InputAxis axes, bool normalise, bool invertX, bool invertY)
    {
        // Limits axes if the input is only meant to record inputs from a single axis
        switch (axes)
        {
            case InputAxis.Horizontal:
                initialInput.y = 0;
                break;
            case InputAxis.Vertical:
                initialInput.x = 0;
                break;
        }

        // Inverts axes
        if (invertX)
        {
            initialInput.x = -initialInput.x;
        }
        if (invertY)
        {
            initialInput.y = -initialInput.y;
        }

        // Normalises input to ensure it doesn't go over the maximum value, unless this is disabled
        if (initialInput.magnitude > 1 && normalise == true)
        {
            initialInput.Normalize();
        }

        return initialInput;
    }



    /// <summary>
    /// NOT TESTED YET. Snaps a vector2 input angle to the closest of a specified number of directions. Useful to simulate digital input while retaining proportional control.
    /// </summary>
    /// <param name="input"></param>
    /// <param name="numberOfDirections"></param>
    /// <returns></returns>
    public static Vector2 SnapInputDirection(Vector2 input, int numberOfDirections)
    {
        float segmentAngle = 360 / numberOfDirections;
        for (float i = 0; i < 360; i += segmentAngle)
        {
            float angle = Vector2.Angle(Vector2.up, input);
            // If the input angle is within a segment
            if (angle > i - i / 2 && angle < i + i / 2)
            {
                // Generate axes for a new Vector2 with the angle of i
                float x = Mathf.Cos(i * Mathf.Deg2Rad);
                float y = Mathf.Sin(i * Mathf.Deg2Rad);
                // Applies axes, then normalises and multiplies to match the original input magnitude
                input = new Vector2(x, y).normalized * input.magnitude;
                i = 360; // Set i to max, ending loop prematurely
            }
        }

        return input;
    }
}

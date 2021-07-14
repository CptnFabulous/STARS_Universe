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
}

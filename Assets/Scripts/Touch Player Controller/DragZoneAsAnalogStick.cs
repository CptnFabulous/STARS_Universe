using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DragZone))]
public class DragZoneAsAnalogStick : MonoBehaviour
{
    DragZone inputZone;
    RectTransform inputZoneTransform;
    
    public Vector2 distanceToMaxInputValue = new Vector2(0.5f, 0.5f);
    // Ensure the input magnitude never goes past 1
    public bool normaliseInput = true;
    // Behaves as if the dimensions of the input area are a square with the dimensions of the shortest edge, ensuring the input is the same length on both axes
    public bool squariseInput = true;

    public InputAxis recordedAxes = InputAxis.Both;
    public bool invertX;
    public bool invertY;

    private void Awake()
    {
        inputZone = GetComponent<DragZone>();
        inputZoneTransform = inputZone.GetComponent<RectTransform>();
    }

    public Vector2 Input()
    {
        Vector2 dragZoneRectDimensions = new Vector2(inputZoneTransform.rect.width, inputZoneTransform.rect.height);
        if (squariseInput == true)
        {
            float shortestEdge = Mathf.Min(dragZoneRectDimensions.x, dragZoneRectDimensions.y);
            dragZoneRectDimensions = new Vector2(shortestEdge, shortestEdge);
        }
        Vector2 inputRect = distanceToMaxInputValue * dragZoneRectDimensions;

        // Divides the drag distance by the hypothetical input rect to get the 
        Vector2 input = inputZone.DragDirectionFromOrigin() / inputRect;

        input = TouchFunction.LimitProcessedInput(input, recordedAxes, normaliseInput, invertX, invertY);

        return input;
    }
}

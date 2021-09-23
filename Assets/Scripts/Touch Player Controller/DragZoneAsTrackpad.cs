using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DragZone))]
public class DragZoneAsTrackpad : MonoBehaviour
{
    DragZone inputZone;
    RectTransform inputZoneTransform;

    public Vector2 distanceToInputOfOne = new Vector2(0.5f, 0.5f);
    
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

    public Vector2 InputValue
    {
        get
        {
            Vector2 dragZoneRectDimensions = new Vector2(inputZoneTransform.rect.width, inputZoneTransform.rect.height);
            if (squariseInput == true)
            {
                float shortestEdge = Mathf.Min(inputZoneTransform.rect.width, inputZoneTransform.rect.height);
                dragZoneRectDimensions = new Vector2(shortestEdge, shortestEdge);
            }
            Vector2 inputRect = distanceToInputOfOne * dragZoneRectDimensions;

            Vector2 input = inputZone.DragDeltaPosition / inputRect;
            //input = new Vector2(input.x / inputRect.x, input.y / inputRect.y);

            input = TouchFunction.LimitProcessedInput(input, recordedAxes, false, invertX, invertY);

            return input;
        }
        
        
    }
}

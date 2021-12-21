using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipNavigationGUI : MonoBehaviour
{
    public SpaceshipMovement controller;


    [Header("Axis colours")]
    public Color colourPlusX = Color.red;
    public Color colourPlusY = Color.green;
    public Color colourPlusZ = Color.blue;
    public Color colourMinusX = Color.cyan;
    public Color colourMinusY = Color.magenta;
    public Color colourMinusZ = Color.yellow;

    [Header("Compass")]
    public Transform compassTransform;
    public Renderer compassPlusX;
    public Renderer compassPlusY;
    public Renderer compassPlusZ;
    public Renderer compassMinusX;
    public Renderer compassMinusY;
    public Renderer compassMinusZ;
    
    void Start()
    {
        // Sets compass colours
        compassPlusX.material.color = colourPlusX;
        compassPlusY.material.color = colourPlusY;
        compassPlusZ.material.color = colourPlusZ;
        compassMinusX.material.color = colourMinusX;
        compassMinusY.material.color = colourMinusY;
        compassMinusZ.material.color = colourMinusZ;
    }
    
    void LateUpdate()
    {
        // Identify relative rotation from camera orientation to point at compass position
        // Rotates compass world position to zero, plus the relative orientation to account for the compass being at a different angle from the camera
        Camera c = controller.viewCamera;
        compassTransform.rotation = Quaternion.FromToRotation(c.transform.forward, compassTransform.position - c.transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipHUD : MonoBehaviour
{
    public SpaceshipMovement controller;

    [Header("HUD elements")]
    public Text speedometer;

    [Header("Compass")]
    public Transform compass;
    public Renderer plusX;
    public Renderer plusY;
    public Renderer plusZ;
    public Renderer minusX;
    public Renderer minusY;
    public Renderer minusZ;


    private void LateUpdate()
    {
        //speedometer.text = MiscMath.RoundToDecimalPlaces(rb.velocity.magnitude, 1) + "km/h";
        speedometer.text = Mathf.RoundToInt(controller.rb.velocity.magnitude) + "km/h";

        Camera c = controller.viewCamera;
        // Identify relative rotation from camera orientation to point at compass position
        Quaternion fromCameraToCompass = Quaternion.FromToRotation(c.transform.forward, compass.position - c.transform.position);
        // Rotates compass to zero, plus the relative orientation to account for the compass being at a different angle from the camera
        compass.rotation = fromCameraToCompass;
        // Sets compass colours
        plusX.material.color = PlanetGrid.colourPlusX;
        plusY.material.color = PlanetGrid.colourPlusY;
        plusZ.material.color = PlanetGrid.colourPlusZ;
        minusX.material.color = PlanetGrid.colourMinusX;
        minusY.material.color = PlanetGrid.colourMinusY;
        minusZ.material.color = PlanetGrid.colourMinusZ;
    }
}

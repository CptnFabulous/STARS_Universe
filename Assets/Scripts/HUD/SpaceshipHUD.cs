using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipHUD : MonoBehaviour
{
    public SpaceshipMovement controller;

    [Header("HUD elements")]
    public Text speedometer;
    public string speedMeasurement = "u/s";



    private void LateUpdate()
    {
        speedometer.text = Mathf.RoundToInt(controller.rb.velocity.magnitude) + speedMeasurement;
    }
}

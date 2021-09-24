using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZeroGravityControlOptions : MonoBehaviour
{
    [Header("General elements")]
    public Button apply;
    public Button revert;

    [Header("Specific options")]
    public Toggle enableTouchControls;
    public SensitivitySlider cameraX;
    public SensitivitySlider cameraY;
    public SensitivitySlider cameraZ;
    public SensitivitySlider gyroX;
    public SensitivitySlider gyroY;
    public SensitivitySlider gyroZ;

    FirstPersonZeroGravityController player;

    private void Awake()
    {
        player = GetComponentInParent<FirstPersonZeroGravityController>();
    }

    void RefreshMenu()
    {
        // If KB+M or touch support is unavailable, grey out and select the appropriate option
        enableTouchControls.interactable = true;
        enableTouchControls.isOn = player.useTouchInputs;

        bool keyboardAndMouseAvailable = Input.mousePresent;
        bool touchControlsAvailable = Input.touchSupported;
        if (!keyboardAndMouseAvailable)
        {
            enableTouchControls.interactable = false;
            enableTouchControls.isOn = true;
        }
        else if (!touchControlsAvailable)
        {
            enableTouchControls.interactable = false;
            enableTouchControls.isOn = false;
        }

        cameraX.Refresh(player.rotationDegreesPerSecond.x);
        cameraY.Refresh(player.rotationDegreesPerSecond.y);
        cameraZ.Refresh(player.rotationDegreesPerSecond.z);

        gyroX.Refresh(player.gyroSensitivity.x);
        gyroY.Refresh(player.gyroSensitivity.y);
        gyroZ.Refresh(player.gyroSensitivity.z);
        gyroX.SetInteractable(SystemInfo.supportsGyroscope);
        gyroY.SetInteractable(SystemInfo.supportsGyroscope);
        gyroZ.SetInteractable(SystemInfo.supportsGyroscope);

    }


    public void ApplySettings(FirstPersonZeroGravityController player)
    {
        player.useTouchInputs = enableTouchControls.isOn;
        player.rotationDegreesPerSecond.x = cameraX.Value;
        player.rotationDegreesPerSecond.y = cameraY.Value;
        player.rotationDegreesPerSecond.z = cameraZ.Value;
        player.rotationDegreesPerSecond.x = gyroX.Value;
        player.rotationDegreesPerSecond.y = gyroY.Value;
        player.rotationDegreesPerSecond.z = gyroZ.Value;
        
    }
}

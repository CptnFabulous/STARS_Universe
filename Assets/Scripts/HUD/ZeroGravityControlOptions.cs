using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZeroGravityControlOptions : OptionsMenu
{
    [Header("Specific options")]
    public Toggle enableTouchControls;
    public SensitivitySlider cameraX;
    public SensitivitySlider cameraY;
    public SensitivitySlider cameraZ;
    public float maxCameraSensitivity = 100;
    public SensitivitySlider gyroX;
    public SensitivitySlider gyroY;
    public SensitivitySlider gyroZ;
    public float maxGyroSensitivity = 5;

    FirstPersonZeroGravityController player;

    public override void SetupOptions()
    {
        enableTouchControls.onValueChanged.AddListener((_) => OnOptionsChanged());
        cameraX.onValueChanged.AddListener((_) => OnOptionsChanged());
        cameraY.onValueChanged.AddListener((_) => OnOptionsChanged());
        cameraZ.onValueChanged.AddListener((_) => OnOptionsChanged());
        gyroX.onValueChanged.AddListener((_) => OnOptionsChanged());
        gyroY.onValueChanged.AddListener((_) => OnOptionsChanged());
        gyroZ.onValueChanged.AddListener((_) => OnOptionsChanged());
    }
    public override void ObtainCurrentValues()
    {
        player = GetComponentInParent<FirstPersonZeroGravityController>();
        if (player == null)
        {
            return;
        }

        // Set touch/KB+M toggle to the player's current setting
        enableTouchControls.interactable = true;
        enableTouchControls.isOn = player.useTouchInputs;

        // If either option is unavailable, grey out and force to the single available option
        if (!Input.mousePresent)
        {
            enableTouchControls.interactable = false;
            enableTouchControls.isOn = true;
        }
        else if (!Input.touchSupported)
        {
            enableTouchControls.interactable = false;
            enableTouchControls.isOn = false;
        }

        cameraX.Refresh(player.rotationDegreesPerSecond.x, maxCameraSensitivity);
        cameraY.Refresh(player.rotationDegreesPerSecond.y, maxCameraSensitivity);
        cameraZ.Refresh(player.rotationDegreesPerSecond.z, maxCameraSensitivity);

        gyroX.Refresh(player.gyroSensitivity.x, maxGyroSensitivity);
        gyroY.Refresh(player.gyroSensitivity.y, maxGyroSensitivity);
        gyroZ.Refresh(player.gyroSensitivity.z, maxGyroSensitivity);
        gyroX.SetInteractable(SystemInfo.supportsGyroscope);
        gyroY.SetInteractable(SystemInfo.supportsGyroscope);
        gyroZ.SetInteractable(SystemInfo.supportsGyroscope);
    }
    public override void ApplySettings()
    {
        player.useTouchInputs = enableTouchControls.isOn;
        // Alter rotation settings
        player.rotationDegreesPerSecond.x = cameraX.Value;
        player.rotationDegreesPerSecond.y = cameraY.Value;
        player.rotationDegreesPerSecond.z = cameraZ.Value;
        // Alter gyro sensitivity
        player.gyroSensitivity.x = gyroX.Value;
        player.gyroSensitivity.y = gyroY.Value;
        player.gyroSensitivity.z = gyroZ.Value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceshipControlOptions : OptionsMenu
{
    [Header("Specific options")]
    public Toggle useTouchControls;

    [Header("Invert Steering")]
    public Toggle invertPitch;
    public Toggle invertYaw;
    public Toggle invertRoll;

    [Header("Mouse")]
    public Slider mousePitch;
    public Slider mouseYaw;
    public float maxMouseSensitivity = 1;

    [Header("Gyro")]
    public Slider gyroPitch;
    public Slider gyroYaw;
    public Slider gyroRoll;
    public float maxGyroSensitivity = 5;

    SpaceshipMovement player;

    public override void ApplySettings()
    {
        player.useTouchInputs = useTouchControls.isOn;

        player.invertPitch = invertPitch.isOn;
        player.invertYaw = invertYaw.isOn;
        player.invertRoll = invertRoll.isOn;

        player.mouseSensitivity.x = OptionsMenu.SliderValueToSensitivity(mouseYaw, maxMouseSensitivity);
        player.mouseSensitivity.y = OptionsMenu.SliderValueToSensitivity(mousePitch, maxMouseSensitivity);
        
        player.gyroSensitivity.x = OptionsMenu.SliderValueToSensitivity(gyroPitch, maxGyroSensitivity);
        player.gyroSensitivity.y = OptionsMenu.SliderValueToSensitivity(gyroYaw, maxGyroSensitivity);
        player.gyroSensitivity.z = OptionsMenu.SliderValueToSensitivity(gyroRoll, maxGyroSensitivity);

        Debug.Log("Spaceship settings applied on frame " + Time.frameCount);
    }

    public override void ObtainCurrentValues()
    {
        player = GetComponentInParent<SpaceshipMovement>();
        if (player == null)
        {
            return;
        }

        // Set touch/KB+M toggle to the player's current setting
        useTouchControls.interactable = true;
        useTouchControls.isOn = player.useTouchInputs;

        // If either option is unavailable, grey out and force to the single available option
        if (!Input.mousePresent)
        {
            useTouchControls.interactable = false;
            useTouchControls.isOn = true;
        }
        else if (!Input.touchSupported)
        {
            useTouchControls.interactable = false;
            useTouchControls.isOn = false;
        }

        invertPitch.isOn = player.invertPitch;
        invertYaw.isOn = player.invertYaw;
        invertRoll.isOn = player.invertRoll;

        OptionsMenu.SensitivityToSliderValue(mouseYaw, player.mouseSensitivity.x, maxMouseSensitivity);
        OptionsMenu.SensitivityToSliderValue(mousePitch, player.mouseSensitivity.y, maxMouseSensitivity);

        OptionsMenu.SensitivityToSliderValue(gyroPitch, player.gyroSensitivity.x, maxGyroSensitivity);
        OptionsMenu.SensitivityToSliderValue(gyroYaw, player.gyroSensitivity.y, maxGyroSensitivity);
        OptionsMenu.SensitivityToSliderValue(gyroRoll, player.gyroSensitivity.z, maxGyroSensitivity);
        gyroPitch.interactable = SystemInfo.supportsGyroscope;
        gyroYaw.interactable = SystemInfo.supportsGyroscope;
        gyroRoll.interactable = SystemInfo.supportsGyroscope;
    }

    public override void SetupOptions()
    {
        useTouchControls.onValueChanged.AddListener((_) => OnOptionsChanged());

        invertPitch.onValueChanged.AddListener((_) => OnOptionsChanged());
        invertYaw.onValueChanged.AddListener((_) => OnOptionsChanged());
        invertRoll.onValueChanged.AddListener((_) => OnOptionsChanged());

        mouseYaw.onValueChanged.AddListener((_) => OnOptionsChanged());
        mousePitch.onValueChanged.AddListener((_) => OnOptionsChanged());

        gyroPitch.onValueChanged.AddListener((_) => OnOptionsChanged());
        gyroYaw.onValueChanged.AddListener((_) => OnOptionsChanged());
        gyroRoll.onValueChanged.AddListener((_) => OnOptionsChanged());
    }
}

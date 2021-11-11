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
        enableTouchControls.onValueChanged.AddListener((_)=> OnOptionsChanged());
        cameraX.onValueChanged.AddListener((_) => OnOptionsChanged());
        cameraY.onValueChanged.AddListener((_) => OnOptionsChanged());
        cameraZ.onValueChanged.AddListener((_) => OnOptionsChanged());
        gyroX.onValueChanged.AddListener((_) => OnOptionsChanged());
        gyroY.onValueChanged.AddListener((_) => OnOptionsChanged());
        gyroZ.onValueChanged.AddListener((_) => OnOptionsChanged());

        apply.onClick.AddListener(() => ApplySettings());
        revert.onClick.AddListener(() => RefreshMenu());
    }
    private void OnEnable()
    {
        if (player != null)
        {
            RefreshMenu();
        }
    }
    private void Start()
    {
        player = GetComponentInParent<FirstPersonZeroGravityController>();
        RefreshMenu();
    }

    void OnOptionsChanged()
    {
        apply.interactable = true;
        revert.interactable = true;
    }
    void RefreshMenu()
    {
        /*
        if (player == null)
        {
            player = GetComponentInParent<FirstPersonZeroGravityController>();
        }

        Debug.Log(this + ", " + player);
        */

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

        cameraX.Refresh(player.rotationDegreesPerSecond.x);
        cameraY.Refresh(player.rotationDegreesPerSecond.y);
        cameraZ.Refresh(player.rotationDegreesPerSecond.z);

        gyroX.Refresh(player.gyroSensitivity.x);
        gyroY.Refresh(player.gyroSensitivity.y);
        gyroZ.Refresh(player.gyroSensitivity.z);
        gyroX.SetInteractable(SystemInfo.supportsGyroscope);
        gyroY.SetInteractable(SystemInfo.supportsGyroscope);
        gyroZ.SetInteractable(SystemInfo.supportsGyroscope);

        apply.interactable = false;
        revert.interactable = false;
    }
    public void ApplySettings()
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
        RefreshMenu();
    }
}

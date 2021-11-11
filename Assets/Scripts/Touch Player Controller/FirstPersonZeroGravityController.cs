using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class FirstPersonZeroGravityController : MonoBehaviour
{
    PlayerHandler player;
    Rigidbody rb;

    public bool useTouchInputs;

    [Header("Camera and rotation")]
    public Vector3 rotationDegreesPerSecond = new Vector3(120, 120, 120);
    public VirtualAnalogStick cameraJoystick;
    public VirtualAnalogStick zRotationJoystick;
    Vector3 rotationValues;

    [Header("Gyro rotation")]
    public Toggle toggleGyro;
    public Vector3 gyroSensitivity = new Vector3(2, 2, 2);

    [Header("Movement")]
    public float moveSpeed = 150;
    public VirtualAnalogStick movementJoystick;
    public VirtualAnalogStick verticalMovementJoystick;
    public Toggle toggleBoost;
    Vector3 movementValues;
    float Speed
    {
        get
        {
            float speed = moveSpeed;
            if (isBoosting)
            {
                speed *= boostMultiplier;
            }
            return speed;
        }
    }

    [Header("Boosting")]
    public float boostMultiplier = 4;
    bool isBoosting;


    // Use this for initialization
    void Awake()
    {
        player = GetComponent<PlayerHandler>();
        rb = GetComponent<Rigidbody>();

        
        toggleBoost.onValueChanged.AddListener((value)=> isBoosting = value);
        // Add a listener so pressing the gyro control button will enable/disable the device's gyro functionality
        toggleGyro.onValueChanged.AddListener((enabled) => Input.gyro.enabled = enabled && SystemInfo.supportsGyroscope);
        toggleGyro.onValueChanged.Invoke(toggleGyro.isOn);


        
    }

    public void SetControlsToComputerOrMobile()
    {
        Debug.Log("Setting controls");

        // Disable touch inputs if not possible on current hardware
        if (Input.touchSupported == false)
        {
            useTouchInputs = false;
        }

        if (useTouchInputs)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = useTouchInputs;

        movementJoystick.gameObject.SetActive(useTouchInputs);
        verticalMovementJoystick.gameObject.SetActive(useTouchInputs);
        cameraJoystick.gameObject.SetActive(useTouchInputs);
        zRotationJoystick.gameObject.SetActive(useTouchInputs);

        //toggleBoost.gameObject.SetActive(useTouchInputs);

        // Adds a secondary check to only enable the gyro controls if the device actually has them
        toggleGyro.gameObject.SetActive(useTouchInputs && SystemInfo.supportsGyroscope);
        player.PauseHandler.pauseButton.gameObject.SetActive(useTouchInputs);
    }





    // Update is called once per frame
    void Update()
    {
        // Records either keyboard + mouse or touch inputs
        if (useTouchInputs)
        {
            movementValues = new Vector3(movementJoystick.Input.x, verticalMovementJoystick.Input.y, movementJoystick.Input.y) * Speed;
            rotationValues = new Vector3(-cameraJoystick.Input.y * rotationDegreesPerSecond.x, cameraJoystick.Input.x * rotationDegreesPerSecond.y, zRotationJoystick.Input.y * rotationDegreesPerSecond.z) * Time.deltaTime;

            if (Input.gyro.enabled) // Even if this is disabled, it still registers previous values. Therefore, only apply gyro rotation if it is specifically enabled
            {
                Vector3 gyroInput = Input.gyro.rotationRate;
                gyroInput.x *= gyroSensitivity.x;
                gyroInput.y *= gyroSensitivity.y;
                gyroInput.z *= gyroSensitivity.z;
                rotationValues += gyroInput;
            }
        }
        else
        {
            // If boost toggle button is pressed, toggle boost speed by invoking functions already set up in the touchscreen control
            if (Input.GetButtonDown("Boost"))
            {
                toggleBoost.isOn = !toggleBoost.isOn;
                toggleBoost.onValueChanged.Invoke(toggleBoost.isOn);
            }

            movementValues = new Vector3(Input.GetAxis("Left/Right"), Input.GetAxis("Up/Down"), Input.GetAxis("Forward/Backward")) * Speed;
            rotationValues = new Vector3(-Input.GetAxis("Mouse Y") * rotationDegreesPerSecond.x, Input.GetAxis("Mouse X") * rotationDegreesPerSecond.y, Input.GetAxis("Clockwise/Counterclockwise") * rotationDegreesPerSecond.z) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.rotation * movementValues * Time.deltaTime);
        rb.MoveRotation(transform.rotation * Quaternion.Euler(rotationValues));
    }
}

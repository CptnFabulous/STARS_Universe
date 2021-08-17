using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    PlayerHandler player;
    
    [Header("Control inputs")]
    public bool useTouchInputs;
    public VirtualAnalogStick movementJoystick;
    public VirtualAnalogStick verticalMovementJoystick;
    public VirtualAnalogStick cameraJoystick;
    public VirtualAnalogStick zRotationJoystick;
    public Toggle boostToggle;


    public Vector3 rotationDegreesPerSecond = new Vector3(120, 120, 120);
    public bool invertLookX;
    public bool invertLookY;
    public bool invertLookZ;


    Rigidbody rb;
    Vector3 movementValues;
    Vector3 rotationValues;

    float speed;
    public float moveSpeed = 150;
    public float boostMultiplier = 4;


    // Use this for initialization
    void Awake()
    {
        player = GetComponent<PlayerHandler>();
        rb = GetComponent<Rigidbody>();

        
        if (boostToggle != null)
        {
            boostToggle.onValueChanged.AddListener(SetSpeed);
        }


        speed = moveSpeed;

        
    }

    public void ToggleBetweenTouchAndComputerControls()
    {
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
        boostToggle.gameObject.SetActive(useTouchInputs);
        player.PauseHandler.pauseButton.gameObject.SetActive(useTouchInputs);

    }

    void SetSpeed(bool isBoosting)
    {
        float newSpeed = moveSpeed;
        if (isBoosting)
        {
            newSpeed *= boostMultiplier;
        }
        speed = newSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Records either keyboard + mouse or touch inputs
        switch(useTouchInputs)
        {
            case true: // Touch inputs
                movementValues = new Vector3(movementJoystick.Input.x, verticalMovementJoystick.Input.y, movementJoystick.Input.y) * speed;
                rotationValues = new Vector3(-cameraJoystick.Input.y * rotationDegreesPerSecond.x, cameraJoystick.Input.x * rotationDegreesPerSecond.y, zRotationJoystick.Input.y * rotationDegreesPerSecond.z) * Time.deltaTime;
                break;
            case false: // KB + M inputs

                // If boost toggle button is pressed, toggle boost speed by invoking functions already set up in the touchscreen control
                if (Input.GetButtonDown("Boost"))
                {
                    boostToggle.isOn = !boostToggle.isOn;
                    boostToggle.onValueChanged.Invoke(boostToggle.isOn);
                }

                movementValues = new Vector3(Input.GetAxis("Left/Right"), Input.GetAxis("Up/Down"), Input.GetAxis("Forward/Backward")) * speed;
                rotationValues = new Vector3(-Input.GetAxis("Mouse Y") * rotationDegreesPerSecond.x, Input.GetAxis("Mouse X") * rotationDegreesPerSecond.y, Input.GetAxis("Clockwise/Counterclockwise") * rotationDegreesPerSecond.z) * Time.deltaTime;

                break;
        }
        
        //This code determines that rotX and Y are based on the mouse X and Y axes, multiplied by the different X and Y sensitivities.
        //rotationValues = new Vector3(cameraJoystick.Input.x * -cameraSensitivity.y, cameraJoystick.Input.x * cameraSensitivity.x, Input.GetAxis("Clockwise/Counterclockwise") * rotateZSpeed * Time.deltaTime);
        //This clamp code keeps the player from moving the camera past 90 or -90 degrees from horizontal, making sure it doesn't move it completely turn around the other way.
        //rotationValues.x = Mathf.Clamp(rotationValues.x, -90f, 90f);
        //rotationValues = new Vector3()
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.rotation * movementValues * Time.deltaTime);
        rb.MoveRotation(transform.rotation * Quaternion.Euler(rotationValues));
    }
}

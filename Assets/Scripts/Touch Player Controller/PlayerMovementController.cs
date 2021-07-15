using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Control inputs")]
    public VirtualAnalogStick movementJoystick;
    public VirtualAnalogStick verticalMovementJoystick;
    public VirtualAnalogStick cameraJoystick;
    public VirtualAnalogStick zRotationJoystick;


    public Vector3 rotationDegreesPerSecond = new Vector3(120, 120, 120);

    Rigidbody rb;
    Vector3 movementValues;
    Vector3 rotationValues;

    float speed;
    public float moveSpeed = 150;
    public float boostMultiplier = 4;


    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        /*
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        */
        speed = moveSpeed;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Boost"))
        {
            speed = moveSpeed * boostMultiplier;
        }
        else
        {
            speed = moveSpeed;
        }

        movementValues = new Vector3(movementJoystick.Input.x, verticalMovementJoystick.Input.y, movementJoystick.Input.y) * speed;
        rotationValues = new Vector3(-cameraJoystick.Input.y * rotationDegreesPerSecond.x, cameraJoystick.Input.x * rotationDegreesPerSecond.y, zRotationJoystick.Input.y * rotationDegreesPerSecond.z) * Time.deltaTime;

        //This code determines that rotX and Y are based on the mouse X and Y axes, multiplied by the different X and Y sensitivities.
        //rotationValues = new Vector3(cameraJoystick.Input.x * -cameraSensitivity.y, cameraJoystick.Input.x * cameraSensitivity.x, Input.GetAxis("Clockwise/Counterclockwise") * rotateZSpeed * Time.deltaTime);
        //This clamp code keeps the player from moving the camera past 90 or -90 degrees from horizontal, making sure it doesn't move it completely turn around the other way.
        //rotationValues.x = Mathf.Clamp(rotationValues.x, -90f, 90f);
        //rotationValues = new Vector3()

        Vector2 cameraInput = cameraJoystick.Input;
        Vector2 rotationInput = zRotationJoystick.Input;

        //transform.Rotate(rotY, rotX, 0);
        //transform.Rotate(0, 0, CA * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 movement = transform.rotation * movementValues;
        rb.MovePosition(transform.position + movement * Time.deltaTime);

        rb.MoveRotation(transform.rotation * Quaternion.Euler(rotationValues));
        

        //rb.MoveRotation(Quaternion.Euler(rotY, rotX, CA * Time.deltaTime));
    }
}

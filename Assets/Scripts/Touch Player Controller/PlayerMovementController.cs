using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Control inputs")]
    public Joystick movementJoystick;
    public DragZone verticalMovementInput;
    public Joystick cameraJoystick;
    public Vector2 cameraSensitivity = new Vector2(2.5f, 2.5f);
    public DragZone forwardAxisRotationInput;

    Rigidbody rb;
    Vector3 movementValues;
    Vector3 rotationValues;

    float speed;
    public float moveSpeed = 150;
    public float boostMultiplier = 4;
    public float rotateZSpeed = 100;


    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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

        movementValues = new Vector3(movementJoystick.Horizontal, 0, movementJoystick.Vertical);

        movementValues = new Vector3(Input.GetAxis("Left/Right"), Input.GetAxis("Up/Down"), Input.GetAxis("Forward/Backward"));

        //This code determines that rotX and Y are based on the mouse X and Y axes, multiplied by the different X and Y sensitivities.
        rotationValues = new Vector3(Input.GetAxis("Mouse Y") * -cameraSensitivity.y, Input.GetAxis("Mouse X") * cameraSensitivity.x, Input.GetAxis("Clockwise/Counterclockwise") * rotateZSpeed * Time.deltaTime);
        //This clamp code keeps the player from moving the camera past 90 or -90 degrees from horizontal, making sure it doesn't move it completely turn around the other way.
        //rotationValues.x = Mathf.Clamp(rotationValues.x, -90f, 90f);


        

        //transform.Rotate(rotY, rotX, 0);
        //transform.Rotate(0, 0, CA * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 movement = transform.rotation * movementValues;
        rb.MovePosition(movement * speed * Time.deltaTime);

        rb.MoveRotation(Quaternion.Euler(rotationValues));

        //rb.MoveRotation(Quaternion.Euler(rotY, rotX, CA * Time.deltaTime));
    }
}

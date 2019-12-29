using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    private CharacterController player;

    float FB;
    float LR;
    float UD;
    float CA;

    float speed;
    public float speedMove;
    public float speedBoost;
    public float speedRot;

    public float sensX;
    public float sensY;
    float rotX;
    float rotY;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        speed = speedMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Boost"))
        {
            speed = speedBoost;
        }
        else
        {
            speed = speedMove;
        }

        FB = Input.GetAxis("Forward/Backward") * speed;
        LR = Input.GetAxis("Left/Right") * speed;
        UD = Input.GetAxis("Up/Down") * speed;
        CA = Input.GetAxis("Clockwise/Counterclockwise") * speedRot;

        //This code determines that rotX and Y are based on the mouse X and Y axes, multiplied by the different X and Y sensitivities. The third line prevents rotY from moving past 90 or -90 degrees.
        rotX = Input.GetAxis("Mouse X") * sensX;
        rotY = Input.GetAxis("Mouse Y") * -sensY;
        //This clamp code keeps the player from moving the camera past 90 or -90 degrees from horizontal, making sure it doesn't move it completely turn around the other way.
        //rotY = Mathf.Clamp(rotY, -90f, 90f);

        Vector3 movement = new Vector3(LR, UD, FB);
        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);

        transform.Rotate(rotY, rotX, 0);
        transform.Rotate(0, 0, CA * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipController : MonoBehaviour
{
    public Vector3 movementSpeed;
    public float rotateSpeed;
    public float boostSpeed;
    public Vector3 cameraRotationSensitivity;
    public Transform cameraAxis;

    Quaternion newRotation;
    Vector3 moveValue;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraInput = new Vector3(-Input.GetAxis("Mouse Y") * cameraRotationSensitivity.y, Input.GetAxis("Mouse X") * cameraRotationSensitivity.x, Input.GetAxis("Clockwise/Counterclockwise") * cameraRotationSensitivity.z); // Obtain input data combined with camera sensitivity
        cameraAxis.transform.Rotate(cameraInput * Time.deltaTime);
        newRotation = cameraAxis.rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);

        
        Vector3 movementInput = new Vector3(Input.GetAxis("Left/Right"), Input.GetAxis("Up/Down"), Input.GetAxis("Forward/Backward"));
        print(movementInput);
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }
        print(movementInput);
        movementInput = new Vector3(movementInput.x * movementSpeed.x, movementInput.y * movementSpeed.y, movementInput.z * movementSpeed.z);
        moveValue = transform.rotation * movementInput;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + moveValue * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        cameraAxis.rotation = newRotation;
    }
}

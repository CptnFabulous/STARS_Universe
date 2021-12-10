using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MovementController
{
    [Header("Mobile controls")]
    public VirtualAnalogStick speedControl;
    public HoldableButton movementBrake;
    public VirtualAnalogStick pitchAndYaw;
    public VirtualAnalogStick roll;
    public HoldableButton rotationBrake;
    public GyroSteeringWheel gyroControls;

    [Header("Movement")]
    public float forwardSpeed = 250;
    public float reverseSpeed = 50;
    public float acceleration = 125;
    public float deceleration = 125;
    public bool autoBrake = true;
    public float MoveInput
    {
        get
        {
            float keyboard = Input.GetAxis("Accelerate/Decelerate");
            float touch = -(speedControl.Input.x + speedControl.Input.y);
            return touch + keyboard;
        }
    }
    public bool BrakingMovement
    {
        get
        {
            return Input.GetButton("Brake Movement") || movementBrake.Held || autoBrake;
        }
    }

    [Header("Steering")]
    public Vector3 steerSpeedPerVelocityUnit = Vector3.one * 0.2f;
    public Vector3 stationaryTurnSpeed = Vector3.one * 60;
    public float angularVelocityDampenSpeed = 1;
    public bool BrakingRotation
    {
        get
        {
            return Input.GetButton("Brake Rotation") || rotationBrake.Held || autoBrake;
        }
    }

    [Header("Camera control")]
    public Vector2 mouseSensitivity = Vector2.one * 0.3f;
    public Vector3 gyroSensitivity = Vector2.one;
    public bool invertPitch;
    public bool invertYaw;
    public bool invertRoll;
    public Camera viewCamera;
    public Transform desiredCameraOrientation;
    public float cameraMoveSpeed = 2;
    public float cameraRotateSpeed = 2;
    Vector3 cameraPosition;
    Vector3 cameraEulerAngles;
    public Vector3 SteerInput
    {
        get
        {
            Vector3 input = Vector3.zero;
            input += new Vector3(-Input.GetAxis("Forward/Backward"), Input.GetAxis("Left/Right"), Input.GetAxis("Clockwise/Counterclockwise"));
            input += new Vector3(-pitchAndYaw.Input.y, pitchAndYaw.Input.x, -(roll.Input.x + roll.Input.y));
            if (gyroControls != null && gyroControls.enabled)
            {
                input += gyroControls.Values;
            }
            if (useTouchInputs == false)
            {
                float pitch = -Input.GetAxis("Mouse Y");
                float yaw = Input.GetAxis("Mouse X");
                float roll = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * float.MaxValue, -1, 1);
                input += new Vector3(pitch, yaw, roll);
            }

            if (invertPitch)
            {
                input.x = -input.x;
            }
            if (invertYaw)
            {
                input.y = -input.y;
            }
            if (invertRoll)
            {
                input.z = -input.z;
            }

            input = MiscMath.Vector3Clamp(input, Vector3.one * -1, Vector3.one);
            //input.Scale(steerSensitivity);

            return input;
        }
    }


    



    public override void Awake()
    {
        base.Awake();
        rb.useGravity = false;
    }
    
    private void FixedUpdate()
    {
        float desiredVelocity = MoveInput;
        if (desiredVelocity != 0)
        {
            if (desiredVelocity > 0)
            {
                desiredVelocity *= forwardSpeed;
            }
            else
            {
                desiredVelocity *= reverseSpeed;
            }
            //rb.AddForce(desiredVelocity * Time.fixedDeltaTime * transform.forward, ForceMode.Force);
            rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVelocity * transform.forward, acceleration * Time.fixedDeltaTime);
        }

        Vector3 steer = stationaryTurnSpeed;
        steer.Scale(SteerInput);
        rb.MoveRotation(transform.rotation * Quaternion.Euler(steer * Time.fixedDeltaTime));

        if (BrakingMovement) // If changing velocity, or braking, shift velocity towards desired speed and direction
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }
        if (steer.magnitude > 0 || BrakingRotation) // If rotation brake is active, slowly counter and cancel current velocity so the player can properly align themselves
        {
            rb.angularVelocity = Vector3.MoveTowards(rb.angularVelocity, Vector3.zero, angularVelocityDampenSpeed * Time.fixedDeltaTime);
        }
    }

    private void LateUpdate()
    {
        float rotateTimer = Time.deltaTime * cameraRotateSpeed;
        /*
        Quaternion relativeRotation = MiscMath.WorldToLocalRotation(desiredCameraOrientation.rotation, transform);
        Vector3 localAngles = relativeRotation.eulerAngles;
        cameraEulerAngles.x = Mathf.LerpAngle(cameraEulerAngles.x, localAngles.x, rotateTimer);
        cameraEulerAngles.y = Mathf.LerpAngle(cameraEulerAngles.y, localAngles.y, rotateTimer);
        cameraEulerAngles.z = Mathf.LerpAngle(cameraEulerAngles.z, localAngles.z, rotateTimer);
        viewCamera.transform.rotation = transform.rotation;// * Quaternion.Euler(cameraEulerAngles);
        */
        
        Vector3 localAngles = desiredCameraOrientation.eulerAngles;
        cameraEulerAngles.x = Mathf.LerpAngle(cameraEulerAngles.x, localAngles.x, rotateTimer);
        cameraEulerAngles.y = Mathf.LerpAngle(cameraEulerAngles.y, localAngles.y, rotateTimer);
        cameraEulerAngles.z = Mathf.LerpAngle(cameraEulerAngles.z, localAngles.z, rotateTimer);
        viewCamera.transform.rotation = Quaternion.Euler(cameraEulerAngles);
        


        float moveTimer = Time.deltaTime * cameraMoveSpeed;
        cameraPosition = Vector3.Lerp(cameraPosition, desiredCameraOrientation.position - transform.position, moveTimer);
        viewCamera.transform.position = cameraPosition + transform.position;

        /*

        // Tether camera position so it doesn't move too far away from the desired orientation
        // Tether camera rotation so it doesn't rotate any further away from the ship than the angle specified by the desired orientation
        
        // Calculate camera position, accounting for physics
        currentCameraPosition = Vector3.SmoothDamp(currentCameraPosition, desiredCameraOrientation.position, ref cameraVelocity, cameraPositionUpdateTime);
        // Calculate camera rotation, accounting for physics
        float timer = Mathf.SmoothDamp(0f, 1f, ref cameraRotationVelocityTimer, cameraRotationUpdateTime);
        currentCameraRotation = Quaternion.Slerp(currentCameraRotation, desiredCameraOrientation.rotation, timer);

        Vector3 position = currentCameraPosition;


        Vector3 fromDesiredToCamera = position - desiredCameraOrientation.position;
        float desiredDistance = Vector3.Distance(desiredCameraOrientation.position, transform.position) * maxCameraTetherDistanceMultiplier;
        if (fromDesiredToCamera.magnitude > desiredDistance)
        {
            fromDesiredToCamera = fromDesiredToCamera.normalized * desiredDistance;
        }
        position = fromDesiredToCamera + desiredCameraOrientation.position;

        // Clamp camera rotation if beyond field of view plus padding
        Quaternion rotation = currentCameraRotation;
        Quaternion rotatedDirectlyTowardsPosition = Quaternion.LookRotation(transform.position - currentCameraPosition, transform.up);
        float angle = Quaternion.Angle(rotation, rotatedDirectlyTowardsPosition);
        float maxAngle = Vector3.Angle(desiredCameraOrientation.forward, transform.position - desiredCameraOrientation.position);
        if (angle > maxAngle)
        {
            rotation = Quaternion.RotateTowards(rotation, rotatedDirectlyTowardsPosition, angle - maxAngle);
        }


        
        

        // Update viewing camera orientation
        viewCamera.transform.rotation = rotation;
        viewCamera.transform.position = position;
        

        
        Vector3 position = currentCameraPosition - desiredCameraOrientation.position;
        float distance = Vector3.Distance(desiredCameraOrientation.position, transform.position);
        if (position.magnitude > distance)
        {
            position = position.normalized * distance;
        }
        position += desiredCameraOrientation.position;
        
        // Update viewing camera orientation
        viewCamera.transform.rotation = currentCameraRotation;
        viewCamera.transform.position = position;

        */
    }

    public override void SetControlsToComputerOrMobile()
    {
        base.SetControlsToComputerOrMobile();
        speedControl.gameObject.SetActive(useTouchInputs);
        movementBrake.gameObject.SetActive(useTouchInputs && autoBrake == false);
        pitchAndYaw.gameObject.SetActive(useTouchInputs && autoBrake == false);
        roll.gameObject.SetActive(useTouchInputs);
        rotationBrake.gameObject.SetActive(useTouchInputs);
    }
}
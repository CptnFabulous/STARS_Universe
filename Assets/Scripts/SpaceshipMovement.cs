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
    public bool autoBrakeVelocity;
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
            return Input.GetButton("Brake Movement") || movementBrake.Held || (autoBrakeVelocity && MoveInput != 0);
        }
    }

    [Header("Steering")]
    public Vector3 steerSpeedPerVelocityUnit = Vector3.one * 0.2f;
    public Vector3 stationaryTurnSpeed = Vector3.one * 60;
    public bool steerIndependentOfVelocity;
    public float angularVelocityDampenSpeed = 1;
    public bool autoBrakeRotation;
    public bool invertPitch;
    public bool invertYaw;
    public bool invertRoll;
    public Vector2 mouseSensitivity = Vector2.one * 0.3f;
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

            return input;
        }
    }
    public bool BrakingRotation
    {
        get
        {
            return Input.GetButton("Brake Rotation") || rotationBrake.Held || autoBrakeRotation;
        }
    }

    [Header("Camera")]
    public Camera viewCamera;
    public Transform desiredCameraOrientation;
    public float cameraMoveSpeed = 2;
    public float cameraRotateSpeed = 2;
    public float rotationForceToBreakCameraTether = 2;
    Vector3 cameraPosition;
    Vector3 cameraEulerAngles;

    [Header("Warping")]
    public SpaceshipWarpMenu warpMenu;

    [Header("Cosmetics")]
    public ParticleSystem exhaust;
    public float exhaustSpeedIdle = 5;
    public float exhaustSpeedMaxed = 20;

    public override void Awake()
    {
        base.Awake();
        rb.useGravity = false;

        warpMenu.ship = this;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Warp") && warpMenu != null && warpMenu.gameObject.activeInHierarchy == false)
        {
            warpMenu.Enter();
        }
    }
    private void FixedUpdate()
    {
        if (manualControlDisabled)
        {
            return;
        }

        #region Obtain speed value and accelerate/reverse
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
        #endregion

        #region Steering
        Vector3 steer = SteerInput * Time.fixedDeltaTime; // Obtains steer input, multiplied by fixed delta time so movement is consistent regardless of framerate and timestep
        if (steerIndependentOfVelocity || rb.velocity.magnitude <= 0) // Multiply by steer speed values
        {
            // If ship is stopped or is allowed to turn regardless of velocity, use default turn value
            steer.Scale(stationaryTurnSpeed);
        }
        else
        {
            // If steering depends on velocity (like a conventional plane), multiply steer speed by current velocity
            steer.Scale(steerSpeedPerVelocityUnit * rb.velocity.magnitude);
            // Rotate velocity as well so plane's movement direction steers properly as well as their rotation. A conventional plane can't strafe!
            float velocitySteerMagnitude = steer.magnitude * Mathf.Deg2Rad;
            rb.velocity = Vector3.RotateTowards(rb.velocity, transform.forward, velocitySteerMagnitude, 0);
        }
        // Rotate plane based on steer vector
        rb.MoveRotation(transform.rotation * Quaternion.Euler(steer));
        #endregion

        #region Brake movement and rotation
        if (BrakingMovement) // If changing velocity, or braking, shift velocity towards desired speed and direction
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }
        if (steer.magnitude > 0 || BrakingRotation) // If rotation brake is active, slowly counter and cancel current velocity so the player can properly align themselves
        {
            rb.angularVelocity = Vector3.MoveTowards(rb.angularVelocity, Vector3.zero, angularVelocityDampenSpeed * Time.fixedDeltaTime);
        }
        #endregion
    }
    private void LateUpdate()
    {
        if (rb.angularVelocity.magnitude < rotationForceToBreakCameraTether)
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

            float moveTimer = Time.deltaTime * cameraMoveSpeed;
            cameraPosition = Vector3.Lerp(cameraPosition, desiredCameraOrientation.position - transform.position, moveTimer);
        }

        viewCamera.transform.rotation = Quaternion.Euler(cameraEulerAngles);
        viewCamera.transform.position = cameraPosition + transform.position;

        
        ParticleSystem.MainModule main = exhaust.main;
        main.startSpeed = Mathf.Lerp(exhaustSpeedIdle, exhaustSpeedMaxed, MoveInput);
    }
    public override void SetControlsToComputerOrMobile()
    {
        base.SetControlsToComputerOrMobile();
        speedControl.gameObject.SetActive(useTouchInputs);
        movementBrake.gameObject.SetActive(useTouchInputs && autoBrakeVelocity == false);
        pitchAndYaw.gameObject.SetActive(useTouchInputs);
        roll.gameObject.SetActive(useTouchInputs);
        rotationBrake.gameObject.SetActive(useTouchInputs && autoBrakeRotation == false);
        gyroControls.resetValues.gameObject.SetActive(useTouchInputs && gyroControls.enabled && SystemInfo.supportsGyroscope);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.collider);
    }
}
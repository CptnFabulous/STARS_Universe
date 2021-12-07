using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MovementController
{

    [Header("Movement")]
    public VirtualAnalogStick speedControl;
    public ButtonWithDownAndUpEvents brake;
    public float forwardSpeed = 150;
    public float reverseSpeed = 50;
    public float acceleration = 50;
    public bool autoBrake = true;
    bool isBraking;
    public float MoveInput
    {
        get
        {
            float keyboard = Input.GetAxis("Accelerate/Decelerate");
            float touch = -(speedControl.Input.x + speedControl.Input.y);
            return touch + keyboard;
        }
    }

    [Header("Steering")]
    public Vector3 steerSpeed = Vector3.one * 60;
    public float angularVelocityDampenSpeed = 50;
    public bool invertPitch;
    public bool invertYaw;
    public bool invertRoll;
    Vector3 steerAngles;
    [Header("Steering - Touch")]
    public VirtualAnalogStick pitchAndYaw;
    public VirtualAnalogStick roll;
    [Header("Steering - Gyro")]
    public bool enableGyro;
    public Vector3 gyroSensitivity = Vector3.one;
    public UnityEngine.UI.Toggle gyroToggle;
    public UnityEngine.UI.Button resetGyro;
    [Header("Steering - Mouse")]
    public Vector2 mouseSensitivity = Vector2.one;
    public float scrollWheelSensitivity = 99;
    //public AnimationCurve mouseSensitivityCurve;

    Vector3 MouseSteerInput()
    {
        Vector2 steer = new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity.x, -Input.GetAxis("Mouse Y") * mouseSensitivity.y);
        steer.x = Mathf.Clamp(steer.x, -1, 1);
        steer.y = Mathf.Clamp(steer.y, -1, 1);

        Vector3 values = new Vector3(steer.y, steer.x, 0);

        values.z = Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * float.MaxValue, -1, 1);
        return values;
    }
    Vector2 mouseInputs;

    Quaternion gyroZero = Quaternion.identity;
    public Vector3 GyroSteer()
    {
        Quaternion relativeGyro = Input.gyro.attitude * Quaternion.Inverse(gyroZero);
        Vector3 eulerAngles = MiscMath.Vector3Multiply(relativeGyro.eulerAngles, gyroSensitivity);
        return MiscMath.Vector3Clamp(eulerAngles, Vector3.one * -1, Vector3.one);
    }
    public void ResetGyro()
    {
        gyroZero = Input.gyro.attitude;
    }

    public Vector3 SteerInput
    {
        get
        {
            Vector3 input = Vector3.zero;
            // Keyboard input
            input += new Vector3(-Input.GetAxis("Forward/Backward"), Input.GetAxis("Left/Right"), Input.GetAxis("Clockwise/Counterclockwise"));
            // Touchscreen input
            input += new Vector3(-pitchAndYaw.Input.y, pitchAndYaw.Input.x, -(roll.Input.x + roll.Input.y));
            if (enableGyro)
            {
                input += GyroSteer();
            }
            if (useTouchInputs == false)
            {
                input += MouseSteerInput();
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
            return input;
        }
    }

    [Header("Camera")]
    public Camera viewCamera;
    public Transform desiredCameraOrientation;
    public float cameraPositionUpdateTime = 0.01f;
    public float cameraRotationUpdateTime = 0.1f;
    Vector3 currentCameraPosition;
    Quaternion currentCameraRotation;

    Vector3 cameraVelocity;
    float cameraRotationVelocityTimer;

    [Header("Warping")]
    public float warpRotateTime = 1;
    public float warpDelayTime = 1;
    public float warpTravelTime = 1;
    public float warpPaddingDistance = 20f;
    public MeshRenderer testMesh;


    public override void Awake()
    {
        base.Awake();
        rb.useGravity = false;
        // Use this for initialization

        // Add a listener so pressing the gyro control button will enable/disable the device's gyro functionality
        gyroToggle.onValueChanged.AddListener((enabled) => Input.gyro.enabled = enabled && SystemInfo.supportsGyroscope);
        gyroToggle.onValueChanged.Invoke(gyroToggle.isOn);
    }
        Input.gyro.enabled = true;

        resetGyro.onClick.AddListener(ResetGyro);
        brake.onDown.AddListener(() => isBraking = true);
        brake.onUp.AddListener(() => isBraking = false);

    // Update is called once per frame
    void Update()
    {
        if (manualControlDisabled)
        {
            steerAngles = Vector3.zero;
            return;
        }

        steerAngles = SteerInput;

        if (Input.GetButtonDown("Warp") && testMesh != null)
        {
            InitiateAutomaticAction(Warp(testMesh.bounds));
        }

    }

    private void FixedUpdate()
    {
        if (!manualControlDisabled)
        {
            float desiredVelocity = MoveInput;
            if (desiredVelocity > 0)
            {
                desiredVelocity *= forwardSpeed;
            }
            else
            {
                desiredVelocity *= reverseSpeed;
            };

            // If velocity is being updated OR
            // Keyboard brake button is being held OR
            // Braking bool has been activated by touch function OR
            // Auto-braking is enabled
            if (desiredVelocity != 0 || Input.GetButton("Brake") || isBraking || autoBrake)
            {
                rb.velocity = Vector3.MoveTowards(rb.velocity, transform.forward * desiredVelocity, acceleration * Time.fixedDeltaTime);//rb.MovePosition(transform.position + distanceToMove * transform.forward);
            }
            
            Quaternion steerRotation = Quaternion.Euler(MiscMath.Vector3Multiply(steerAngles, steerSpeed) * Time.fixedDeltaTime);
            rb.MoveRotation(transform.rotation * steerRotation);
            if (steerAngles.magnitude > 0) // If player is steering and also moving due to velocity, slowly counter and cancel current velocity so the player can properly align themselves
            {
                rb.angularVelocity = Vector3.MoveTowards(rb.angularVelocity, Vector3.zero, angularVelocityDampenSpeed * steerAngles.magnitude * Time.fixedDeltaTime);
            }
        }
        
        // Calculate camera position, accounting for physics
        currentCameraPosition = Vector3.SmoothDamp(currentCameraPosition, desiredCameraOrientation.position, ref cameraVelocity, cameraPositionUpdateTime);
        // Calculate camera rotation, accounting for physics
        float timer = Mathf.SmoothDamp(0f, 1f, ref cameraRotationVelocityTimer, cameraRotationUpdateTime);
        currentCameraRotation = Quaternion.Slerp(currentCameraRotation, desiredCameraOrientation.rotation, timer);
    }

    private void LateUpdate()
    {
        // Produce a viewing camera position whose distance is clamped relative to the player transform, so it doesn't get too far away when the player is moving at high speeds.
        float correctDistance = Vector3.Distance(transform.position, desiredCameraOrientation.position);
        Vector3 relativePositionWithCorrectDistance = (currentCameraPosition - transform.position).normalized * correctDistance;

        // Update viewing camera orientation
        viewCamera.transform.rotation = currentCameraRotation;
        viewCamera.transform.position = transform.position + relativePositionWithCorrectDistance;

    }

    public override void SetControlsToComputerOrMobile()
    {
        base.SetControlsToComputerOrMobile();
        speedControl.gameObject.SetActive(useTouchInputs);
        brake.gameObject.SetActive(useTouchInputs);
        pitchAndYaw.gameObject.SetActive(useTouchInputs);
        roll.gameObject.SetActive(useTouchInputs);

        gyroToggle.gameObject.SetActive(useTouchInputs && SystemInfo.supportsGyroscope);
        resetGyro.gameObject.SetActive(useTouchInputs && SystemInfo.supportsGyroscope);
    }





    public IEnumerator Warp(Bounds thingToWarpTo)
    {
        rb.isKinematic = true;
        
        Quaternion oldRotation = transform.rotation;
        Quaternion lookingTowardsDestination = Quaternion.LookRotation(thingToWarpTo.center - transform.position);

        float timer = 0;
        while (timer != 1)
        {
            timer += Time.deltaTime / warpRotateTime;
            timer = Mathf.Clamp01(timer);

            transform.rotation = Quaternion.Lerp(oldRotation, lookingTowardsDestination, timer);

            yield return null;
        }

        yield return new WaitForSeconds(warpDelayTime);

        Vector3 oldPosition = transform.position;
        Vector3 destinationPoint = (oldPosition - thingToWarpTo.center).normalized * (thingToWarpTo.extents.magnitude + warpPaddingDistance);

        timer = 0;
        while (timer != 1)
        {
            timer += Time.deltaTime / warpTravelTime;
            timer = Mathf.Clamp01(timer);

            transform.position = Vector3.Lerp(oldPosition, destinationPoint, timer);

            yield return null;
        }

        manualControlDisabled = false;

        rb.isKinematic = false;
    }
}

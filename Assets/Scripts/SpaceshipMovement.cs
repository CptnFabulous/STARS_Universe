using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMovement : MovementController
{

    [Header("Movement")]
    public VirtualAnalogStick speedControl;
    public float forwardSpeed = 150;
    public float reverseSpeed = 50;
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
    public bool invertPitch;
    public bool invertYaw;
    public bool invertRoll;
    Vector3 steerAngles;
    [Header("Steering - Touch")]
    public VirtualAnalogStick pitchAndYaw;
    public VirtualAnalogStick roll;
    [Header("Steering - Gyro")]
    public Vector3 gyroSensitivity = Vector3.one;
    public UnityEngine.UI.Toggle gyroToggle;
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

        float scrollValue = Input.GetAxis("Mouse ScrollWheel") * float.MaxValue;
        scrollValue = Mathf.Clamp(scrollValue, -1, 1);
        //Debug.Log(scrollValue);
        values.z = scrollValue;
        //Debug.Log(scrollValue);
        return values;
    }
    Vector2 mouseInputs;
    Vector3 GyroSteerInput()
    {
        if (!Input.gyro.enabled)
        {
            return Vector3.zero;
        }
        gyro += MiscMath.Vector3Multiply(Input.gyro.rotationRate, gyroSensitivity);
        gyro = MiscMath.Vector3Clamp(gyro, Vector3.one * -1, Vector3.one);
        Vector3 finalValue = gyro;
        finalValue.x = -finalValue.x;
        return finalValue;
    }
    Vector3 gyro;
    public Vector3 SteerInput
    {
        get
        {
            Vector3 keyboard = new Vector3(-Input.GetAxis("Forward/Backward"), Input.GetAxis("Left/Right"), Input.GetAxis("Clockwise/Counterclockwise"));
            Vector3 mouse = MouseSteerInput();
            Vector3 touch = new Vector3(-pitchAndYaw.Input.y, pitchAndYaw.Input.x, -(roll.Input.x + roll.Input.y));
            Vector3 gyro = GyroSteerInput();

            Vector3 input = keyboard + mouse + touch + gyro;
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


    // Update is called once per frame
    void Update()
    {
        if (manualControlDisabled)
        {
            steerAngles = Vector3.zero;
            return;
        }

        steerAngles = MiscMath.Vector3Multiply(SteerInput, steerSpeed);

        if (Input.GetButtonDown("Boost") && testMesh != null)
        {
            InitiateAutomaticAction(Warp(testMesh.bounds));
        }

    }

    private void FixedUpdate()
    {
        if (!manualControlDisabled)
        {
            float distanceToMove = MoveInput;
            if (distanceToMove > 0)
            {
                distanceToMove *= forwardSpeed;
            }
            else
            {
                distanceToMove *= reverseSpeed;
            }
            distanceToMove *= Time.fixedDeltaTime;
            rb.MovePosition(transform.position + distanceToMove * transform.forward);
            Quaternion steerRotation = Quaternion.Euler(steerAngles * Time.fixedDeltaTime);
            rb.MoveRotation(transform.rotation * steerRotation);
        }
        
        currentCameraPosition = Vector3.SmoothDamp(currentCameraPosition, desiredCameraOrientation.position, ref cameraVelocity, cameraPositionUpdateTime);
        float correctDistance = Vector3.Distance(transform.position, desiredCameraOrientation.position);
        Vector3 relativePositionWithCorrectDistance = (currentCameraPosition - transform.position).normalized * correctDistance;
        currentCameraPosition = transform.position + relativePositionWithCorrectDistance;

        float timer = Mathf.SmoothDamp(0f, 1f, ref cameraRotationVelocityTimer, cameraRotationUpdateTime);
        currentCameraRotation = Quaternion.Slerp(currentCameraRotation, desiredCameraOrientation.rotation, timer);
    }

    private void LateUpdate()
    {
        viewCamera.transform.rotation = currentCameraRotation;
        viewCamera.transform.position = currentCameraPosition;
    }

    public override void SetControlsToComputerOrMobile()
    {
        base.SetControlsToComputerOrMobile();
        speedControl.gameObject.SetActive(useTouchInputs);
        pitchAndYaw.gameObject.SetActive(useTouchInputs);
        roll.gameObject.SetActive(useTouchInputs);

        gyroToggle.gameObject.SetActive(useTouchInputs && SystemInfo.supportsGyroscope);
    }





    public IEnumerator Warp(Bounds thingToWarpTo)
    {
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
    }
}

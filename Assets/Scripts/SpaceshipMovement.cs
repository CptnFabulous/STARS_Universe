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
    public VirtualAnalogStick pitchAndYaw;
    public VirtualAnalogStick roll;
    public Vector3 steerSpeed = Vector3.one * 60;
    Vector3 steerAngles;
    public Vector2 MouseSteer(Vector2 sensitivity, ref Vector2 persistentValue, AnimationCurve curveX, AnimationCurve curveY)
    {
        // Obtain 2D camera input (mouse X and Y or gyro X and Y)
        // Move rollAndPitch based on 2D camera input multiplied by sensitivity
        // Clamp rollAndPitch so they're between 0 and 1

        Vector2 steer = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        steer.x *= sensitivity.x;
        steer.y *= sensitivity.y;
        persistentValue += steer;
        persistentValue.x = Mathf.Clamp(persistentValue.x, -1, 1);
        persistentValue.y = Mathf.Clamp(persistentValue.y, -1, 1);
        persistentValue.x *= curveX.Evaluate(Mathf.Abs(persistentValue.x));
        persistentValue.y *= curveY.Evaluate(Mathf.Abs(persistentValue.y));
        return persistentValue;
    }
    public Vector3 SteerInput
    {
        get
        {
            Vector3 touch = new Vector3(pitchAndYaw.Input.y, pitchAndYaw.Input.x, -(roll.Input.x + roll.Input.y));
            Vector3 keyboard = new Vector3(Input.GetAxis("Forward/Backward"), Input.GetAxis("Left/Right"), Input.GetAxis("Clockwise/Counterclockwise"));
            return touch + keyboard;
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
    }


    // Update is called once per frame
    void Update()
    {
        if (manualControlDisabled)
        {
            steerAngles = Vector3.zero;
            return;
        }

        steerAngles.x = SteerInput.x * steerSpeed.x;
        steerAngles.y = SteerInput.y * steerSpeed.y;
        steerAngles.z = SteerInput.z * steerSpeed.z;

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

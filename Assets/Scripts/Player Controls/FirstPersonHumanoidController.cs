using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class FirstPersonHumanoidController : MonoBehaviour
{
    [Header("Camera")]
    public Transform headTransform;
    public Camera playerCamera;
    public DragZoneAsTrackpad cameraInput;
    public float sensitivityX = 75;
    public float sensitivityY = 75;
    float cameraInputX;
    float cameraInputY;

    [Header("Movement")]
    public DragZoneAsAnalogStick movementInput;
    public float movementSpeed;
    float currentSpeed;
    Vector3 movementValues;

    [Header("Jumping")]
    public Button jumpButton;
    public float jumpForce;
    public float groundingRaycastLength;
    public LayerMask terrainDetection;
    bool willJump;

    bool IsGrounded
    {
        get
        {
            // Casts a ray to determine if the player is standing on solid ground.
            Ray r = new Ray(transform.position + transform.up * (hitbox.height * 0.5f), -transform.up);
            if (Physics.SphereCast(r, hitbox.radius, (hitbox.height * 0.5f) + groundingRaycastLength, terrainDetection))
            {
                return true;
            }
            return false;
        }
    }

    [Header("Crouching")]
    public Button crouchToggleButton;
    public float standHeight = 2;
    public float crouchHeight = 1;
    float relativeHeadHeight = 0.875f;
    public float speedModifierWhileCrouching = 0.5f;
    public float crouchTransitionTime = 0.25f;
    bool isCrouching;
    float crouchTimer;
    
    
    Rigidbody rb;
    CapsuleCollider hitbox;
    IEnumerator currentAutomaticAction; // If the player is performing an action during which movement is temporarily disabled, it is referenced here

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        hitbox = GetComponent<CapsuleCollider>();
        jumpButton.onClick.AddListener(JumpInput);
        crouchToggleButton.onClick.AddListener(()=> InputCrouch(!isCrouching));

        currentSpeed = movementSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector2 input = cameraInput.InputValue;
        cameraInputX = input.x * sensitivityX * Time.deltaTime;
        cameraInputY -= input.y * sensitivityY * Time.deltaTime;
        cameraInputY = Mathf.Clamp(cameraInputY, -90, 90); // Camera.y is then clamped to ensure it does not move past 90* or 90*, ensuring that the player does not flip the camera over completely.
        transform.Rotate(0, cameraInputX, 0); // Player is rotated on y axis based on Camera.x, for turning left and right
        headTransform.localRotation = Quaternion.Euler(cameraInputY, 0, 0); // Player head is rotated in x axis based on Camera.y, for looking up and down

        bool canJump = (willJump == false && IsGrounded);
        jumpButton.interactable = canJump;

        Vector2 moveInput = movementInput.InputValue;
        movementValues = new Vector3(moveInput.x, 0, moveInput.y);

        // Crouching stuff
    }

    public void JumpInput()
    {
        willJump = true;
    }

    public void InputCrouch(bool activate)
    {
        isCrouching = activate;
        StopCoroutine(currentAutomaticAction);
        currentAutomaticAction = ChangeCrouchStats(isCrouching);
        StartCoroutine(currentAutomaticAction);
        
    }

    IEnumerator ChangeCrouchStats(bool active)
    {
        float time = -crouchTransitionTime;
        if (active)
        {
            time = crouchTransitionTime;
        }

        while (crouchTimer < 1 && active || crouchTimer > 0 && !active)
        {
            crouchTimer += Time.deltaTime / time;

            hitbox.height = Mathf.Lerp(standHeight, crouchHeight, crouchTimer);
            headTransform.localPosition = new Vector3(0, hitbox.height * relativeHeadHeight, 0);

            currentSpeed = movementSpeed * Mathf.Lerp(1, speedModifierWhileCrouching, crouchTimer);

            yield return null;
        }


    }

    private void FixedUpdate()
    {
        movementValues = transform.rotation * movementValues;
        rb.MovePosition(transform.position + (movementValues * currentSpeed * Time.deltaTime));

        if (willJump)
        {
            willJump = false;
            rb.AddForce(transform.up * jumpForce);
        }
    }
}

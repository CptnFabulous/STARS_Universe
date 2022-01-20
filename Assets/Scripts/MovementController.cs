using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController : MonoBehaviour
{
    [HideInInspector] public PlayerHandler player;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider c;
    IEnumerator currentAutoAction;

    public bool useTouchInputs;
    public bool forceTouchInputsForDebugging;
    [HideInInspector] public bool manualControlDisabled;

    public virtual void Awake()
    {
        player = GetComponent<PlayerHandler>();
        rb = GetComponent<Rigidbody>();
        c = GetComponent<Collider>();
        useTouchInputs = true;
    }

    public virtual void SetControlsToComputerOrMobile()
    {
        Debug.Log("Setting controls");

        //useTouchInputs = Input.touchSupported || forceTouchInputsForDebugging;

        // Disable touch inputs if not possible on current hardware
        if (Input.touchSupported == false && forceTouchInputsForDebugging == false)
        {
            useTouchInputs = false;
        }

        if (useTouchInputs)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = useTouchInputs;

        player.PauseHandler.pauseButton.gameObject.SetActive(useTouchInputs);
    }


    public void InitiateAutomaticAction(IEnumerator autoAction)
    {
        manualControlDisabled = true;
        if (currentAutoAction != null)
        {
            StopCoroutine(currentAutoAction);
        }
        currentAutoAction = autoAction;
        StartCoroutine(currentAutoAction);
    }
}

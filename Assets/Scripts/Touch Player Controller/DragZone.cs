using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;




[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class DragZone : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool giveValuesInScreenSpace;
    public Color defaultColour = Color.white;
    public Color pressedColour = Color.gray;
    public UnityEvent onDown;
    public UnityEvent onUp;

    // References so stuff works properly
    Image i;
    RectTransform rt;
    Canvas c;
    RectTransform crt;

    // Extra functionality to reset the delta position value because IDragHandler doesn't bloody do a simple thing like that
    IEnumerator checkCoroutine;
    WaitForEndOfFrame wait;
    bool stopDragCheckBool;

    // Up and down data
    public Vector2 DownPosition { get; private set; }
    public Vector2 UpPosition { get; private set; }
    public bool IsPressed { get; private set; }

    // Holding data
    public Vector2 DragCurrentPosition { get; private set; }
    public Vector2 DragDeltaPosition { get; private set; }
    public Vector2 DragDirectionFromOrigin
    {
        get
        {
            return DragCurrentPosition - DownPosition;
        }
    }

    void Awake()
    {
        i = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
        c = rt.GetComponentInParent<Canvas>();
        crt = c.GetComponent<RectTransform>();

        i.color = defaultColour;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;

        // Determine position
        DownPosition = ProcessValues(eventData.position);

        DragCurrentPosition = ProcessValues(eventData.position);
        DragDeltaPosition = Vector2.zero;

        onDown.Invoke();
        i.color = pressedColour;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // If this is the first frame the player has started dragging, create a coroutine to monitor when they stop
        if (checkCoroutine == null)
        {
            checkCoroutine = CheckPlayerHasStoppedDragging();
            StartCoroutine(checkCoroutine);
        }

        DragCurrentPosition = ProcessValues(eventData.position);
        DragDeltaPosition = ProcessValues(eventData.delta);
        stopDragCheckBool = true;
    }

    IEnumerator CheckPlayerHasStoppedDragging()
    {
        // This function activates when the player starts dragging
        stopDragCheckBool = true;
        while (stopDragCheckBool == true)
        {
            // The OnDrag function will set this to true, then this function sets it to false and waits a frame.
            stopDragCheckBool = false;
            yield return wait;
        }
        // If on the next frame the bool has not been turned true again, the player has stopped dragging. Reset DragDeltaPosition and end this function.
        DragDeltaPosition = Vector2.zero;
        checkCoroutine = null;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
        UpPosition = ProcessValues(eventData.position);

        DragCurrentPosition = ProcessValues(eventData.position);
        DragDeltaPosition = Vector2.zero;

        onUp.Invoke();
        i.color = defaultColour;
    }

    Vector2 ProcessValues(Vector2 screenPosition)
    {
        if (giveValuesInScreenSpace == false)
        {
            // Multiply/divide canvas rect width and height in relation to the screen width and height, converting screen dimensions into canvas dimensions
            float x = screenPosition.x / Screen.width * crt.rect.width;
            float y = screenPosition.y / Screen.height * crt.rect.height;
            screenPosition = new Vector2(x, y);

            //Debug.Log("CRT dimensions = " + new Vector2(crt.rect.width, crt.rect.height));
        }
        return screenPosition;
    }

}
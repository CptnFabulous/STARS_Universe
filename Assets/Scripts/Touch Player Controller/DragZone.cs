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
    Image i;
    RectTransform rt;
    Canvas c;
    RectTransform crt;

    [Tooltip("If null, values are given in screen space")]
    public Camera viewingCamera;
    public Color defaultColour = Color.white;
    public Color pressedColour = Color.gray;
    public UnityEvent onDown;
    public UnityEvent onUp;
    
    // Up and down data
    public Vector2 DownPosition { get; private set; }
    public Vector2 UpPosition { get; private set; }
    public bool IsPressed { get; private set; }

    // Holding data
    public Vector2 DragCurrentPosition { get; private set; }
    public Vector2 DragDeltaPosition { get; private set; }
    public Vector2 DragDirectionFromOrigin()
    {
        return DragCurrentPosition - DownPosition;
    }




    /*
    Vector2 CanvasScaleFactors()
    {
        // Multiply/divide canvas rect width and height in relation to the screen width and height
        return new Vector2(crt.rect.width * Screen.width, crt.rect.height * Screen.height);
    }

    Vector2 ProcessScreenSpaceValues(Vector2 screenPosition, bool convertToCanvasSpace)
    {
        if (convertToCanvasSpace == true)
        {
            screenPosition = screenPosition * CanvasScaleFactors();
        }
        return screenPosition;
    }
    */



    void Awake()
    {
        i = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
        c = rt.GetComponentInParent<Canvas>();
        crt = c.GetComponent<RectTransform>();

        i.color = defaultColour;

        Debug.Log(new Vector2(crt.rect.width, crt.rect.height) + ", " + new Vector2(Screen.width, Screen.height) + ", " + new Vector2(crt.rect.width * Screen.width, crt.rect.height * Screen.height));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;

        // Determine position
        DownPosition = eventData.position;

        DragCurrentPosition = eventData.position;
        DragDeltaPosition = Vector2.zero;

        onDown.Invoke();
        i.color = pressedColour;
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragCurrentPosition = eventData.position;
        DragDeltaPosition = eventData.delta;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
        UpPosition = eventData.position;

        DragCurrentPosition = eventData.position;
        DragDeltaPosition = Vector2.zero;

        onUp.Invoke();
        i.color = defaultColour;
    }

    
}
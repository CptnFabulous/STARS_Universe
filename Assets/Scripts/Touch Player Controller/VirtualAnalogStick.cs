using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class VirtualAnalogStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    RectTransform rt;
    Canvas c;
    RectTransform crt;

    public bool normaliseInput = true;
    public InputAxis recordedAxes = InputAxis.Both;
    public bool invertX;
    public bool invertY;
    Image background;
    public Image handle;
    public Color defaultColour = Color.white;
    public Color pressedColour = Color.gray;

    public Vector2 Input { get; private set; }



    void Awake()
    {
        rt = GetComponent<RectTransform>();
        c = GetComponentInParent<Canvas>();
        crt = c.GetComponent<RectTransform>();

        background = GetComponent<Image>();

        ResetCosmetics();
    }

    void ResetCosmetics()
    {
        handle.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        handle.rectTransform.SetParent(background.transform);
        handle.rectTransform.anchoredPosition = Vector2.zero;
        
        background.color = defaultColour;
        handle.color = defaultColour;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.color = pressedColour;
        handle.color = pressedColour;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 inputValue = eventData.position;
        // Multiply/divide canvas rect width and height in relation to the screen width and height, converting screen dimensions into canvas dimensions
        float x = inputValue.x / Screen.width * crt.rect.width;
        float y = inputValue.y / Screen.height * crt.rect.height;
        inputValue = new Vector2(x, y);

        //rt.anc

        // Value is minused to get the direction the player's input is in from the centre of the joystick
        //inputValue = inputValue - rt.rect.center;
        
        // Creates a hypothetical rectangle with half the dimensions of the actual analog stick area, to act as a maximum input radius
        Vector2 dragZoneRectDimensions = new Vector2(rt.rect.width, rt.rect.height) * 0.5f;
        // Divides the distance from the centre of the rectangle by the input radius axes, to produce a 0-1 value (value might be over if position is outside the stick zone
        inputValue = new Vector2(inputValue.x / rt.rect.width, inputValue.y / rt.rect.height);

        inputValue = TouchFunction.LimitProcessedInput(inputValue, recordedAxes, normaliseInput, invertX, invertY);

        // Sets final input value
        Input = inputValue; 
        // Updates handle's visual position
        handle.rectTransform.anchoredPosition = inputValue * new Vector2(rt.rect.width, rt.rect.height) * 0.5f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Input = Vector2.zero;
        ResetCosmetics();
    }
}
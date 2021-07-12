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
    
    Image background;
    public Image handle;
    public Color defaultColour;
    public Color pressedColour;

    public Vector2 Input { get; private set; }


    /*
    Vector2 GetInput(Vector2 screenPosition)
    {
        Vector2 centre = rt.rect.
    }
    */

    void Awake()
    {
        rt = GetComponent<RectTransform>();



        background = GetComponent<Image>();
        background.color = defaultColour;
        handle.color = defaultColour;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }


}
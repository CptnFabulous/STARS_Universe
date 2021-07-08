using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class ButtonWithDownAndUpEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public UnityEvent onDown;
    public Color defaultColour = Color.white;
    public UnityEvent onUp;
    public Color pressedColour = Color.gray;
    Image i;

    void Awake()
    {
        i = GetComponent<Image>();
        i.color = defaultColour;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onDown.Invoke();
        i.color = pressedColour;
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onUp.Invoke();
        i.color = defaultColour;
    }

    
}

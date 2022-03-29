using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(Toggle))]
public class SensitivitySlider : MonoBehaviour
{
    public float maxValue = 100;
    
    Slider value;
    Toggle invert;

    public Slider.SliderEvent onValueChanged;

    public float Value
    {
        get
        {
            return GetValue(value, invert, maxValue);
        }
    }
    public void Refresh(float currentSetting, float absoluteMaxValue)
    {
        maxValue = absoluteMaxValue;
        SetValue(value, invert, currentSetting, maxValue);
    }

    public void SetInteractable(bool interactable)
    {
        value.interactable = interactable;
        invert.interactable = interactable;
    }
    private void Awake()
    {
        value = GetComponentInChildren<Slider>();
        invert = GetComponentInChildren<Toggle>();
        value.onValueChanged.AddListener((_) => onValueChanged.Invoke(Value));
        invert.onValueChanged.AddListener((_) => onValueChanged.Invoke(Value));
    }
    
    public static void SetValue(Slider value, Toggle invert, float currentValue, float absoluteMaxValue)
    {
        // Update the options to reflect the current value. If the value is negative, make it positive and set the invert toggle to is on

        float valueToShow = currentValue / absoluteMaxValue; // Turns original value into a -1 to 1 range
        invert.isOn = valueToShow < 0; // If value is negative, update invert toggle accordingly
        valueToShow = Mathf.Abs(valueToShow); // Convert value to abolute 0 - 1 value, since negativity is accounted for by the bool
        valueToShow *= value.maxValue; // Scale 0 - 1 value to match the range of the slider
        value.value = Mathf.Clamp(valueToShow, value.minValue, value.maxValue); // Clamp value and update slider accordingly
    }
    public static float GetValue(Slider value, Toggle invert, float absoluteMaxValue)
    {
        float processedValue = value.value / value.maxValue;
        if (invert.isOn)
        {
            processedValue = -processedValue;
        }
        return processedValue * absoluteMaxValue;
    }
}

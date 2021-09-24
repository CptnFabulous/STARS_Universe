using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(Toggle))]
public class SensitivitySlider : MonoBehaviour
{
    Slider value;
    Toggle invert;

    public Slider.SliderEvent onValueChanged;

    public float Value
    {
        get
        {
            float processedValue = value.value;
            if (invert.isOn)
            {
                processedValue = -processedValue;
            }
            return processedValue;
        }
    }

    public void Refresh(float currentSetting)
    {
        // Update the options to reflect the current value.
        // If the value is negative, make it positive and set the invert toggle to is on
        bool isNegative = currentSetting < 0;
        invert.isOn = isNegative;
        currentSetting = isNegative ? -currentSetting : currentSetting;
        value.value = Mathf.Clamp(currentSetting, value.minValue, value.maxValue);

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

    
}

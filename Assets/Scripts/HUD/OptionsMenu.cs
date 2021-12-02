﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class OptionsMenu : MonoBehaviour
{
    [Header("General elements")]
    public Button apply;
    public Button revert;

    public virtual void Awake()
    {
        apply.onClick.AddListener(() => Apply());
        revert.onClick.AddListener(() => Refresh());
        SetupOptions();
    }
    void OnEnable()
    {
        Refresh();
    }

    public void Apply()
    {
        ApplySettings();
        Refresh();
    }
    void Refresh()
    {
        ObtainCurrentValues();
        apply.interactable = false;
        revert.interactable = false;
    }
    public void OnOptionsChanged()
    {
        apply.interactable = true;
        revert.interactable = true;
    }
    
    /// <summary>
    /// Add listeners to each option to run OnOptionsChanged()
    /// </summary>
    public abstract void SetupOptions();

    /// <summary>
    /// Obtain reference to whatever settings are being altered, and refresh menu options to match
    /// </summary>
    public abstract void ObtainCurrentValues();

    /// <summary>
    /// Apply the values in the menu options to the appropriate setting variables
    /// </summary>
    public abstract void ApplySettings();


    /// <summary>
    /// Update a slider's value to represent a scaled sensitivity value. Some sensitivity values can be very large or small numbers, so this converts them to a slider range designed for better UX
    /// </summary>
    /// <param name="value"></param>
    /// <param name="absoluteMaxValue"></param>
    /// <returns></returns>
    public static float SliderValueToSensitivity(Slider value, float maxValue)
    {
        return value.value / value.maxValue * maxValue;
    }

    /// <summary>
    /// Converts a slider value from the slider's range (optimised for UX) to a value of similar scale but usable for its intended purpose.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="currentValue"></param>
    /// <param name="maxValue"></param>
    public static void SensitivityToSliderValue(Slider value, float currentValue, float maxValue)
    {
        // Update the options to reflect the current value.
        float valueToShow = currentValue / maxValue * value.maxValue; // Turns original value into a -1 to 1 range
        value.value = Mathf.Clamp(valueToShow, value.minValue, value.maxValue); // Clamp value and update slider accordingly
    }
}

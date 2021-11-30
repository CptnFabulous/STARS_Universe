using System.Collections;
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
        apply.onClick.AddListener(() => ApplySettings());
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


}

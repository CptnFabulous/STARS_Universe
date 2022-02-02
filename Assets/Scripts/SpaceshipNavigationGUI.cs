using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipNavigationGUI : MonoBehaviour
{
    public SpaceshipMovement controller;

    [Header("Compass")]
    public Color[] axisColours = new Color[]
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.cyan,
        Color.magenta,
        Color.yellow
    };
    public Renderer[] compassAxes;
    public Transform compassTransform;

    [Header("Complex GUI")]
    public LineRenderer gridLineTemplate;
    LineRenderer[] gridLines;
    public static readonly Vector3[] axes = new Vector3[6]
    {
        Vector3.right,
        Vector3.up,
        Vector3.forward,
        Vector3.left,
        Vector3.down,
        Vector3.back
    };
    public void ToggleComplexGUI()
    {
        ComplexGUIActive = !ComplexGUIActive;
    }
    public bool ComplexGUIActive
    {
        get
        {
            return complexGUIActive;
        }
        set
        {
            if (value == complexGUIActive)
            {
                return;
            }

            complexGUIActive = value;

            for (int i = 0; i < axes.Length; i++)
            {
                gridLines[i].gameObject.SetActive(complexGUIActive);
            }
        }
    }
    bool complexGUIActive;

    private void OnValidate()
    {
        if (axisColours.Length != axes.Length)
        {
            System.Array.Resize(ref axisColours, axes.Length);
        }
        if (compassAxes.Length != axes.Length)
        {
            System.Array.Resize(ref compassAxes, axes.Length);
        }
    }

    private void Awake()
    {
        gridLines = new LineRenderer[axes.Length];
        for (int i = 0; i < axes.Length; i++)
        {
            // Set name and colour of axis
            gridLines[i] = (i == 0) ? gridLineTemplate : Instantiate(gridLineTemplate, transform);
            gridLines[i].name = "Grid line: " + axes[i];
            gridLines[i].colorGradient = new Gradient()
            {
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(axisColours[i], 0)
                },
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1, 0)
                }
            };

            // Set positions and length
            gridLines[i].SetPosition(0, axes[i] * SolarSystem.Current.minSafeDistanceFromSun);
            gridLines[i].SetPosition(1, axes[i] * SolarSystem.Current.radius);

            // Prematurely disable
            gridLines[i].gameObject.SetActive(false);

            // Set compass colours
            compassAxes[i].material.color = axisColours[i];
        }
    }
    void LateUpdate()
    {
        // Identify relative rotation from camera orientation to point at compass position
        // Rotates compass world position to zero, plus the relative orientation to account for the compass being at a different angle from the camera
        Camera c = controller.viewCamera;
        compassTransform.rotation = Quaternion.FromToRotation(c.transform.forward, compassTransform.position - c.transform.position);
    }



    
}

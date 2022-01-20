using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGrid : MonoBehaviour
{
    public static Color colourPlusX = Color.red;
    public static Color colourPlusY = Color.green;
    public static Color colourPlusZ = Color.blue;
    public static Color colourMinusX = Color.cyan;
    public static Color colourMinusY = Color.magenta;
    public static Color colourMinusZ = Color.yellow;

    public float radius = 10;
    public int numberOfRings;
    public int maxPointsInRadiusCircle = 100;
    public float ringThickness;
    public Material ringMaterial;

    LineRenderer[] x;
    LineRenderer[] y;
    LineRenderer[] z;

    private void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Gradient xGradient = new Gradient();
        xGradient.mode = GradientMode.Blend;
        xGradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(colourPlusZ, 0),
            new GradientColorKey(colourMinusY, 0.25f),
            new GradientColorKey(colourMinusZ, 0.5f),
            new GradientColorKey(colourPlusY, 0.75f),
            new GradientColorKey(colourPlusZ, 1),
        };

        Gradient yGradient = new Gradient();
        yGradient.mode = GradientMode.Blend;
        yGradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(colourPlusZ, 0),
            new GradientColorKey(colourPlusX, 0.25f),
            new GradientColorKey(colourMinusZ, 0.5f),
            new GradientColorKey(colourMinusX, 0.75f),
            new GradientColorKey(colourPlusZ, 1),
        };

        Gradient zGradient = new Gradient();
        zGradient.mode = GradientMode.Blend;
        zGradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(colourPlusY, 0),
            new GradientColorKey(colourMinusX, 0.25f),
            new GradientColorKey(colourMinusY, 0.5f),
            new GradientColorKey(colourPlusX, 0.75f),
            new GradientColorKey(colourPlusX, 1),
        };

        

        x = GenerateConcentricRings(transform, numberOfRings, radius, maxPointsInRadiusCircle, ringThickness, xGradient, ringMaterial, transform.forward, transform.right);
        y = GenerateConcentricRings(transform, numberOfRings, radius, maxPointsInRadiusCircle, ringThickness, yGradient, ringMaterial, transform.forward, transform.up);
        z = GenerateConcentricRings(transform, numberOfRings, radius, maxPointsInRadiusCircle, ringThickness, zGradient, ringMaterial, transform.up, transform.forward);
    }

    public void GenerateRingRender(LineRenderer l, float radius, int numberOfPoints, Vector3 forward, Vector3 up)
    {
        Vector3[] points = new Vector3[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            points[i] = Quaternion.AngleAxis(360 / numberOfPoints * i, up) * forward;
            points[i] *= radius;
        }
        l.positionCount = numberOfPoints;
        l.SetPositions(points);
    }
    LineRenderer[] GenerateConcentricRings(Transform origin, int numberOfRings, float radius, int maxNumberOfPoints, float thickness, Gradient colours, Material ringMaterial, Vector3 forward, Vector3 up)
    {
        LineRenderer[] r = new LineRenderer[numberOfRings];
        for (int i = 0; i < numberOfRings; i++)
        {
            GameObject newRenderer = new GameObject();
            newRenderer.name = "Ring " + (i + 1) + "/" + numberOfRings;
            newRenderer.transform.parent = origin;
            r[i] = newRenderer.AddComponent<LineRenderer>();
            Debug.Log(r[i]);
            r[i].useWorldSpace = false;
            r[i].loop = true;
            r[i].material = ringMaterial;
            r[i].widthMultiplier = thickness;
            r[i].colorGradient = colours;
            GenerateRingRender(r[i], radius / numberOfRings * (i + 1), Mathf.RoundToInt(maxNumberOfPoints / numberOfRings * (i + 1)), forward, up);
        }
        return r;
    }
}

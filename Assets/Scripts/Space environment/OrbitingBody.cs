using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingBody : MonoBehaviour
{
    [Header("Orbiting")]
    public float orbitSpeedInDegrees = 60;
    public Vector3 orbitAxisEulerAngles = Vector3.up;

    [Header("Spin")]
    public float spinSpeed = 15;

    [Header("Cosmetics")]
    public LineRenderer orbitPath;
    public int segmentNumber = 180;

    // Start is called before the first frame update
    void Start()
    {
        Transform orbitAxisTransform = Instantiate(new GameObject(), transform.parent).transform;
        orbitAxisTransform.name = name + "'s orbit axis";
        orbitAxisTransform.localPosition = Vector3.zero;
        orbitAxisTransform.localRotation = Quaternion.LookRotation(transform.position - transform.parent.position, Quaternion.Euler(orbitAxisEulerAngles) * Vector3.up);
        transform.parent = orbitAxisTransform;


        if (orbitPath != null)
        {
            orbitPath.transform.parent = orbitAxisTransform;
            orbitPath.transform.localPosition = Vector3.zero;
            orbitPath.transform.localRotation = Quaternion.identity;
            orbitPath.transform.localScale = Vector3.one;
            orbitPath.useWorldSpace = false;
            orbitPath.loop = true;

            RenderRing(orbitPath, Vector3.Distance(Vector3.zero, transform.localPosition), segmentNumber);
            //RenderRing(orbitPath, Vector3.Distance(Vector3.zero, transform.localPosition), minSegmentNumber, maxSegmentDistance);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.Rotate(new Vector3(0, orbitSpeedInDegrees * Time.deltaTime));
        transform.RotateAround(transform.position, transform.up, spinSpeed * Time.deltaTime);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.parent.position, 500 * (Quaternion.Euler(orbitAxisEulerAngles) * Vector3.forward));
    }



    public static void RenderRing(LineRenderer renderer, float radius, int numberOfSegments/*int minSegmentNumber, float maxSegmentLength*/)
    {
        renderer.useWorldSpace = false;
        renderer.loop = true;

        //float circumference = radius * 2 * Mathf.PI;
        //int numberOfSegments = Mathf.RoundToInt(circumference / maxSegmentLength);
        Vector3[] points = new Vector3[numberOfSegments]; // Calculates number of segments and makes a Vector3 array
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = radius * (Quaternion.Euler(0, 360 / points.Length * i, 0) * Vector3.forward);
            Debug.DrawLine(renderer.transform.position, renderer.transform.TransformPoint(points[i]), Color.magenta, 50);
        }
        Debug.Log(points.Length);
        renderer.positionCount = points.Length;
        renderer.SetPositions(points);
    }
}

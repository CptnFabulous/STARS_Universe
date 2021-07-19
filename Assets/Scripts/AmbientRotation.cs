using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientRotation : MonoBehaviour
{
    public float rotationSpeed;
    public Vector3 axes;
    public bool randomiseAxes;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (randomiseAxes == true)
        {
            axes = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axes, rotationSpeed * Time.deltaTime);
    }
}

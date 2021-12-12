using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroSteeringWheel : MonoBehaviour
{
    public Vector3 sensitivity = Vector3.one * 0.2f;
    public UnityEngine.UI.Button resetValues;

    public Vector3 Values
    {
        get
        {
            return angles;
        }
    }
    Vector3 angles;

    public Gyroscope gyro;

    private void Awake()
    {
        resetValues.onClick.AddListener(ResetValues);
    }

    private void Start()
    {
        if (gyro == null)
        {
            gyro = Input.gyro;
        }
        gyro.enabled = true;
    }


    private void OnEnable()
    {
        if (SystemInfo.supportsGyroscope == false)
        {
            enabled = false;
        }
        ResetValues();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rate = gyro.rotationRate;
        rate.Scale(sensitivity);
        angles += rate;
        angles = MiscMath.Vector3Clamp(angles, Vector3.one * -1, Vector3.one);
    }

    public void ResetValues()
    {
        angles = Vector3.zero;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public float radius = 5000;
    public Collider sun;
    public float minSafeDistanceFromSun = 2000;

    public static SolarSystem Current
    {
        get
        {
            if (currentInstance == null)
            {
                currentInstance = FindObjectOfType<SolarSystem>();
            }
            return currentInstance;
        }
    }
    static SolarSystem currentInstance;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planetSettings : MonoBehaviour
{

    public Transform player;
    public Color colour;
    float sizeCurrent;
    float playerDistance;

    // Use this for initialization
    void Awake()
    {
        GetComponent<Renderer>().material.color = colour;
    }

    /*
    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);
    }
    */
}

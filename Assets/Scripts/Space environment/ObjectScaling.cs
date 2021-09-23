using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaling : MonoBehaviour {

    public Transform player;
    public Color colour;
    public float scale = 1;
    float sizeX;
    float sizeY;
    float sizeZ;
    float sizeCurrent;
    float playerDistance;

    // Use this for initialization
    void Start()
    {
        sizeX = transform.localScale.x;
        sizeY = transform.localScale.y;
        sizeZ = transform.localScale.z;
        //GetComponent<Renderer>().material.color = colour;
    }

    // Update is called once per frame
    void Update()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);
        //sizeCurrent = size / playerDistance;
        transform.localScale = new Vector3(sizeX, sizeY, sizeZ) * scale / playerDistance;
    }
}

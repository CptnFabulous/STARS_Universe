using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSettings : MonoBehaviour {

    public Transform player;
    public Color colour;
    public float size = 1000;
    float sizeCurrent;
    float playerDistance;

    // Use this for initialization
    void Start ()
    {
        GetComponent<Renderer>().material.color = colour;
    }
	
	// Update is called once per frame
	void Update ()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);
        sizeCurrent = size / playerDistance;
        transform.localScale = new Vector3(sizeCurrent, sizeCurrent, sizeCurrent);
    }
}

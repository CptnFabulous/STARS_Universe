using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSize : MonoBehaviour {

    public Transform player;
    public float size = 1000;
    float sizeCurrent;
    float playerDistance;
    


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);
        sizeCurrent = size / playerDistance;
        transform.localScale = new Vector3(sizeCurrent, sizeCurrent, sizeCurrent);
    }
}

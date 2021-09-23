using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planet_Sprite : MonoBehaviour {

    public Transform player;
    
    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(player);
	}
}

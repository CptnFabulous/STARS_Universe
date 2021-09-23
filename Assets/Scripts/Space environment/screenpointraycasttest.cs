using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenpointraycasttest : MonoBehaviour {

    RaycastHit mouseOver;
    
    // Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Ray Cursor = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast (Cursor, out mouseOver, 20f))
        {
            if (mouseOver.collider.tag == "Player")
            {
                print("You're mousing over a " + mouseOver.collider.name + "! The cube is " + mouseOver.distance + " units away.");
            }
        }
	}
}

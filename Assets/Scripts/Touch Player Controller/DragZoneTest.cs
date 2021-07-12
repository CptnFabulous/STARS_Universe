using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragZoneTest : MonoBehaviour
{
    public DragZone zone;
    public LineRenderer directionalLine;
    public Camera viewingCamera;

    public Transform referenceCube;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(zone.IsPressed + ", " + zone.DragCurrentPosition + ", " + zone.DragDeltaPosition);
        if (zone.IsPressed)
        {
            directionalLine.enabled = true;

            referenceCube.position = viewingCamera.ScreenToWorldPoint(new Vector3(zone.DownPosition.x, zone.DownPosition.y));

            Vector3[] positions = new Vector3[]
            {
                //Vector3.zero,
                //new Vector3(zone.DragDirectionFromOrigin().x, zone.DragDirectionFromOrigin().y)
                viewingCamera.ScreenToWorldPoint(new Vector3(zone.DownPosition.x, zone.DownPosition.y)),
                viewingCamera.ScreenToWorldPoint(new Vector3(zone.DragCurrentPosition.x, zone.DragCurrentPosition.y))

            };

            directionalLine.SetPositions(positions);
        }
        else
        {
            directionalLine.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragZoneTest : MonoBehaviour
{
    public DragZone zone;
    public DragZoneAsAnalogStick stick;

    public DragZoneAsTrackpad pad;
    Vector2 inputValue = Vector2.zero;



    public Text debugText;

    RectTransform zoneRect;



    // Start is called before the first frame update
    void Start()
    {
        zoneRect = zone.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        //Vector2 screenZeroToOneValues = new Vector2(zone.DragCurrentPosition.x / Screen.width, zone.DragCurrentPosition.y / Screen.height);

        //Debug.Log(zone.IsPressed + ", " + zone.DragCurrentPosition + ", " + zone.DragDeltaPosition);

        //Debug.Log("Pressed = " + zone.IsPressed + ", " + Misc.LogVector2Fine(zone.DragCurrentPosition) + ", " + zone.DragDeltaPosition);
        //Debug.Log(zone.IsPressed + ", " + Misc.LogVector2Fine(screenZeroToOneValues) + ", " + Misc.LogVector2Fine(zone.DragDeltaPosition));
        //debugText.text = "";
        //debugText.text += stick.Input().ToString() + "\n";


        //debugText.text = stick.Input().ToString();
        //debugText.text = Misc.LogVector2Fine(pad.Input());


        //Debug.Log(zone.DragDeltaPosition);
        //inputValue += pad.Input();
        //debugText.text = Misc.LogVector2Fine(inputValue);




        if (zone.IsPressed)
        {
            
            
            
            /*
            Vector2 distanceToMaxInputValue = new Vector2(0.5f, 0.5f);
            Vector2 dimensionsOfHypotheticalRectangle = new Vector2(zoneRect.rect.width * distanceToMaxInputValue.x, zoneRect.rect.height * distanceToMaxInputValue.y);

            Vector2 ddfo = zone.DragDirectionFromOrigin();
            
            
            ddfo = new Vector2(ddfo.x / dimensionsOfHypotheticalRectangle.x, ddfo.y / dimensionsOfHypotheticalRectangle.y);
            */

            
            
            
            
            
            
            
            
            
            
            
            
            /*
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
            */
        }
        else
        {
            //directionalLine.enabled = false;
            //debugText.text = "";
        }
    }
}

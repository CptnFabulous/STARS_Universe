using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScavengerHuntMinigame : Minigame
{
    public GameObject objectToFind;

    public int numberOfCloseByObjectsToList = 3;
    public string distanceUnitsName = "units";

    public string distanceDescription = "It's {distance} {units} from {nearbyObject}, in a {direction} direction.";

    string hintText;



    public override void StartGame(PlayerHandler newPlayer)
    {
        base.StartGame(newPlayer);

        // Provide hint as to object location

        List<MeshRenderer> nearbyVisibleObjects = new List<MeshRenderer>(FindObjectsOfType<MeshRenderer>());
        nearbyVisibleObjects.Sort((a, b) =>
        {
            float distanceA = Vector3.Distance(a.bounds.center, objectToFind.transform.position);
            float distanceB = Vector3.Distance(b.bounds.center, objectToFind.transform.position);
            return distanceA.CompareTo(distanceB);
        });
        nearbyVisibleObjects = nearbyVisibleObjects.GetRange(0, numberOfCloseByObjectsToList); // Shorten list down to a specified number of closest objects

        hintText = "Nearby objects:";
        for (int i = 0; i < nearbyVisibleObjects.Count; i++)
        {
            float distance = Vector3.Distance(nearbyVisibleObjects[i].bounds.center, objectToFind.transform.position);

            string newLine = distanceDescription;
            newLine.Replace("{distance}", Mathf.RoundToInt(distance).ToString());
            newLine.Replace("{units}", distanceUnitsName);
            newLine.Replace("{nearbyObject}", nearbyVisibleObjects[i].name);
            hintText += "\n" + newLine;
        }

    }

    public override void SetupHUD(MinigameHeadsUpDisplay hud)
    {
        
    }
    public override void UpdateHUD(MinigameHeadsUpDisplay hud)
    {
        
    }
}

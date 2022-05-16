using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StuntFlightMinigame : Minigame
{
    [Header("Stunt Flight Variables")]
    public Transform[] rings;
    public GameObject nextRingIndicator;
    public PhysicsEventTrigger nextRingTrigger;

    public int currentRingIndex
    {
        get => index;
        private set
        {
            index = value;
            // Orients and enables the indicator to match the current ring (or disables if the index is outside the array)
            nextRingIndicator.SetActive(index >= 0 && index < rings.Length);
            if (nextRingIndicator.activeInHierarchy)
            {
                nextRingIndicator.transform.position = rings[currentRingIndex].position;
                nextRingIndicator.transform.rotation = rings[currentRingIndex].rotation;
            }
        }
    }
    int index;

    private void Awake()
    {
        nextRingTrigger.onTriggerEnter.AddListener(OnCurrentRingEnter);
        nextRingTrigger.collider.isTrigger = true;
    }
    public void OnCurrentRingEnter(Collider c)
    {
        // Checks if the thing hitting the trigger is a player
        PlayerHandler ph = c.GetComponentInParent<PlayerHandler>();
        if (ph == null)
        {
            return;
        }
        // Only continue if the current player matches, or if no player is assigned
        if (currentlyPlaying != null && ph != currentlyPlaying)
        {
            return;
        }
        // If entry angle is greater than 90, player has entered the ring from the wrong direction.
        float entryAngle = Vector3.Angle(ph.transform.forward, rings[currentRingIndex].forward);
        if (entryAngle >= 90)
        {
            return;
        }
        
        // A valid player has passed through. If no player is assigned, start the game
        if (currentRingIndex == 0)
        {
            StartGame(ph);
        }

        // Increment progress
        Progress();
    }

    public override void ResetGame()
    {
        base.ResetGame();
        currentRingIndex = 0;
    }
    public override void StartGame(PlayerHandler newPlayer)
    {
        base.StartGame(newPlayer);

        

    }
    public override void Progress()
    {
        currentRingIndex++;
        if (currentRingIndex >= rings.Length)
        {
            Win(); // Player has passed through the final ring
        }
        else
        {
            onProgress.Invoke();
        }
    }
    public override void SetupHUD(MinigameHeadsUpDisplay hud)
    {
        hud.timerObject.SetActive(true);
        hud.counterObject.SetActive(true);
    }
    public override void UpdateHUD(MinigameHeadsUpDisplay hud)
    {
        TimeValue time = new TimeValue(timeLimit.InSeconds - Time.time - startTime);
        hud.timer.text = time.ToString(hud.timerDecimalPlaces);
    }
}

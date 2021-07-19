using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPauseManager : MonoBehaviour
{
    





    public void TimeCheckOnPause()
    {
        //Debug.Log("Checking all players are paused");
        bool everybodyIsPaused = true;
        PlayerHandler[] players = FindObjectsOfType<PlayerHandler>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].PauseHandler.CurrentState != PlayerState.InPauseMenu)
            {
                everybodyIsPaused = false;
                i = players.Length;
            }
        }

        if (everybodyIsPaused == true)
        {
            //Debug.Log("All players are paused, setting timescale to zero");
            Time.timeScale = 0;
        }
    }

    public void TimeCheckOnResume()
    {
        //Debug.Log("Resuming time");
        Time.timeScale = 1;
    }
}

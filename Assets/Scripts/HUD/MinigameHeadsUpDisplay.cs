using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameHeadsUpDisplay : MonoBehaviour
{
    [Header("Timer")]
    public GameObject timerObject;
    public Text timer;
    public int timerDecimalPlaces = 2;

    [Header("Counter")]
    public GameObject counterObject;
    public Text counterDescription;
    public Text counter;

    public Minigame CurrentMinigame
    {
        get => current;
        set
        {
            if (current == value) return;
            current = value;
            enabled = current != null;
            if (current == null) return;

            CurrentMinigame.SetupHUD(this);
        }
    }
    Minigame current;

    void LateUpdate()
    {
        if (CurrentMinigame == null)
        {
            enabled = false;
            return;
        }

        CurrentMinigame.UpdateHUD(this);
    }
    private void OnDisable()
    {
        timerObject.SetActive(false);
        counterObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariableHeadsUpDisplay : MonoBehaviour
{
    public GameObject timerObject;
    public Text timer;
    int timerDecimalPlaces = 2;

    StuntFlightMinigame currentMinigame;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (currentMinigame != null)
        {
            TimeValue time = new TimeValue(Time.time - currentMinigame.startTime);
            timer.text = time.ToString(timerDecimalPlaces);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public PlayerMovementController controls;
    



    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */



    public void PauseGame()
    {
        PauseMenu menu = GameObject.FindObjectOfType<PauseMenu>();

        menu.Pause();


    }
}

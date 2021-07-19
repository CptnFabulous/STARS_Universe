using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public PlayerMovementController Controls { get; private set; }
    public GameStateHandler PauseHandler { get; private set; }

    private void Awake()
    {
        Controls = GetComponent<PlayerMovementController>();
        PauseHandler = GetComponent<GameStateHandler>();
    }

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





}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public FirstPersonZeroGravityController Controls { get; private set; }
    public GameStateHandler PauseHandler { get; private set; }

    private void Awake()
    {
        Controls = GetComponent<FirstPersonZeroGravityController>();
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

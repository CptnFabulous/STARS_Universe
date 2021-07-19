using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PlayerState
{
    Active,
    InPauseMenu,
    InGameMenus,
    Failed
}

[RequireComponent(typeof(PlayerHandler))]
public class GameStateHandler : MonoBehaviour
{
    PlayerHandler playerHandler;

    public PlayerState CurrentState { get; private set; }

    [Header("Heads-up display")]
    public Canvas headsUpDisplay;

    [Header("Pause menu")]
    public Canvas pauseMenu;
    public Button pauseButton;
    public Button resumeButton;



    private void Awake()
    {
        playerHandler = GetComponent<PlayerHandler>();
    }


    private void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);

        // Create GlobalPauseManager
        GlobalPauseManager gpm = FindObjectOfType<GlobalPauseManager>();
        if (gpm == null)
        {
            GameObject gpmObject = new GameObject("Global Pause Manager");
            gpm = gpmObject.AddComponent<GlobalPauseManager>();
        }

        Debug.Log("Pause menu setup stuff done");

        ResumeGame();
    }

    void SwitchMenus(Canvas correctMenu)
    {
        // Disable all menus (this will help in case another menu is active when it shouldn't be)
        headsUpDisplay.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);

        // Enable the correct menu
        correctMenu.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        //Debug.Log("Pausing game");
        
        SwitchMenus(pauseMenu);
        playerHandler.Controls.enabled = false;

        CurrentState = PlayerState.InPauseMenu;
        GlobalPauseManager cr = FindObjectOfType<GlobalPauseManager>();
        cr.TimeCheckOnPause();
    }

    public void ResumeGame()
    {
        //Debug.Log("Resuming game");

        SwitchMenus(headsUpDisplay);
        playerHandler.Controls.enabled = true;

        CurrentState = PlayerState.Active;
        GlobalPauseManager cp = FindObjectOfType<GlobalPauseManager>();
        cp.TimeCheckOnResume();
    }

    


    
}

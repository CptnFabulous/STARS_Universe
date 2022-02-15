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
        // Adds listeners so the buttons work properly
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
    }
    private void OnEnable()
    {
        // Pre-emptively resumes the game to ensure everything is set up correctly
        ResumeGame();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Pause") && CurrentState == PlayerState.Active)
        {
            PauseGame();
        }
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
        SwitchMenus(pauseMenu);
        playerHandler.Controls.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        CurrentState = PlayerState.InPauseMenu;

        #region Time adjustment
        // Checks all players to see if any of them are not paused
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

        // If all players are paused
        if (everybodyIsPaused == true)
        {
            // Pause time
            Time.timeScale = 0;
        }
        #endregion
    }

    public void ResumeGame()
    {
        if (playerHandler == null)
        {
            return;
        }
        
        SwitchMenus(headsUpDisplay);
        
        Debug.Log("Resuming game");
        playerHandler.Controls.enabled = true;
        playerHandler.Controls.SetControlsToComputerOrMobile();

        CurrentState = PlayerState.Active;

        #region Time adjustment
        Time.timeScale = 1;
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    /*
    public Button button_Resume;
    public Button button_Quit;
    */
    public KeyCode button_Pause = KeyCode.Escape;
    bool isPaused;
    public GameObject player;
    public Canvas menu;

    // Use this for initialization
    void Start ()
    {
        Pause();
        /*
        Button resume = button_Resume.GetComponent<Button>();
        Button quit = button_Quit.GetComponent<Button>();
        resume.onClick.AddListener(Resume);
        quit.onClick.AddListener(Quit);
        */
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(button_Pause) && isPaused == false)
        {
            Pause();
        }

        else if (Input.GetKeyDown(button_Pause) && isPaused == true)
        {
            Resume();
        }
    }

    public void Pause ()
    {
        Time.timeScale = 0.0f;
        player.GetComponent<playerMovement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        menu.enabled = true;
        isPaused = true;
    }

    public void Resume ()
    {
        Time.timeScale = 1.0f;
        player.GetComponent<playerMovement>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        menu.enabled = false;
        isPaused = false;
    }

    public void Quit ()
    {
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
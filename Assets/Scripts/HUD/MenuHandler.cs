using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
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





    public void SwitchMenu(Canvas menu)
    {
        
    }

    public void LoadSceneSimply(string name)
    {
        SceneManager.LoadScene(name);
    }

    IEnumerator LoadingScreen(string sceneToLoad, string loadingScreenString)
    {
        // Pause the game and bring up the loading screen
        Time.timeScale = 0;
        SceneManager.LoadScene(loadingScreenString);
        
        // Start loading the new scene
        AsyncOperation loadNewScene = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        WaitForEndOfFrame updateLoop = new WaitForEndOfFrame();
        while (loadNewScene.isDone == false)
        {
            // Update loop!
            // Play a looping animation so the player knows the game hasn't crashed
            // Update progress bar using loadNewScene.progress as a reference

            yield return updateLoop;
        }

        // Hide progress bar, bring up 'press this to continue' window

        bool activateThisToContinue = false;
        while (activateThisToContinue == false)
        {
            if (Input.GetButtonDown("Submit"))
            {
                activateThisToContinue = true;
            }

            yield return updateLoop;
        }


        // Get rid of the loading screen and unpause the game.
        SceneManager.UnloadSceneAsync(loadingScreenString);
        Time.timeScale = 1;
    }

    public void ResetTime(float scale)
    {
        Time.timeScale = scale;
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

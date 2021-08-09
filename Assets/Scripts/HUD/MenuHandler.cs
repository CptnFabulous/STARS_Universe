using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuHandler : MonoBehaviour
{
    /*
    public Canvas parentMenu;
    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    */
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




    /*
    public void SwitchMenu(MenuHandler newMenu)
    {
        MenuHandler root = newMenu;
        // If 'root' has a parent, it isn't the root.
        while(root.parentMenu != null)
        {
            //Assign the parent as the new root and check again until a menu is reached with no parent
            root = root.parentMenu;
        }
    }
    */

    public void SwitchMenu(Canvas newMenu)
    {
        Canvas[] childMenus = newMenu.rootCanvas.GetComponentsInChildren<Canvas>();
        for(int i = 0; i < childMenus.Length; i++)
        {
            childMenus[i].enabled = false;
        }
        newMenu.enabled = true;
    }

    public void ReturnToParentMenu()
    {
        //SwitchMenu(parentMenu);
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

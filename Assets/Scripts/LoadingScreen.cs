using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadingScreen : MonoBehaviour
{
    #region Static loading elements
    static string sceneToLoad;
    static List<System.Func<bool>> criteriaToFinish = new List<System.Func<bool>>();
    static int criteriaCompleted;

    public static void LoadScene(string newScene, string loadingScreenScene = "Loading Screen")
    {
        sceneToLoad = newScene;
        SceneManager.LoadSceneAsync(loadingScreenScene);
    }

    public static void AddCriteriaToFulfil(System.Func<bool> criteria)
    {
        criteriaToFinish.Add(criteria);
    }
    #endregion

    [Header("If no level is established")]
    public string defaultMenuIfNoneIsSpecified = "Main Menu";

    #region Loading screen object code
    [Header("UI elements")]
    public Text newSceneName;
    public Image progressBar;
    public Text percentage;
    public Text currentAction;

    [Header("On load started")]
    public UnityEvent onLoadStart;
    [Header("On load finished")]
    public Button enterLevelButton;
    public UnityEvent onLoadFinished;
    [Header("On level enter")]
    public UnityEvent onLevelEnter;

    
    AsyncOperation load;
    Scene oldLevel;
    bool readyToEnterNewLevel;

    private void Awake()
    {
        enterLevelButton.onClick.AddListener(() => readyToEnterNewLevel = true);
    }
    private void Start()
    {
        if (sceneToLoad == null || sceneToLoad == "")
        {
            sceneToLoad = defaultMenuIfNoneIsSpecified;
        }

        newSceneName.text = sceneToLoad;

        //StartCoroutine(SceneLoadSequence());
        StartCoroutine(LoadSceneAndInitialProcesses());
    }



    private void LateUpdate()
    {
        float visibleLoadValue = Mathf.Clamp01(load.progress / 0.9f);


        visibleLoadValue *= 0.5f;
        if (criteriaToFinish.Count > 0)
        {
            float taskCompletedPercentage = criteriaCompleted / criteriaToFinish.Count;
            taskCompletedPercentage /= 0.5f;
            visibleLoadValue += taskCompletedPercentage;
        }

        progressBar.fillAmount = visibleLoadValue;
        percentage.text = (visibleLoadValue * 100) + "%";
    }

    IEnumerator LoadSceneAndInitialProcesses()
    {
        // Start loading scene additively
        // Activate scene
        readyToEnterNewLevel = false;
        oldLevel = SceneManager.GetActiveScene();
        load = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        //load.allowSceneActivation = false;
        onLoadStart.Invoke();

        yield return new WaitUntil(() => load.isDone);

        // Eliminate/disable players and immediately pause game
        GameStateHandler[] players = FindObjectsOfType<GameStateHandler>();
        for (int i = 0; i < players.Length; i++)
        {
            //Destroy(handlers[i].gameObject);
            players[i].gameObject.SetActive(false);
        }

        Debug.Log("Letting processes finish before enabling player control, on frame " + Time.frameCount);

        // Waits for objects in the new scene to start running functions that need to occur before entering the level properly
        yield return new WaitForEndOfFrame();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Wait until all criteria in 'criteriaToFinish' are true
        yield return new WaitUntil(() =>
        {
            int completed = 0;
            for (int i = 0; i < criteriaToFinish.Count; i++)
            {
                if (criteriaToFinish[i].Invoke() == true)
                {
                    completed++;
                }
            }
            criteriaCompleted = completed;
            return criteriaCompleted >= criteriaToFinish.Count;
        });
        criteriaToFinish.Clear(); // Once check has finished, reset criteria

        // Now that all processes are finished, wait until player presses the button to load the scene
        enterLevelButton.gameObject.SetActive(true);
        enterLevelButton.interactable = true;
        onLoadFinished.Invoke();
        yield return new WaitUntil(() => readyToEnterNewLevel == true);

        // Unpause game and re-enable players
        Time.timeScale = 1;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.SetActive(true);
        }

        Debug.Log("Unloading old level: " + oldLevel.name);
        SceneManager.UnloadSceneAsync(oldLevel);

        Debug.Log("Level processes finished, resuming control on frame " + Time.frameCount);
    }
    IEnumerator SimpleSceneLoadSequence()
    {
        readyToEnterNewLevel = false;
        oldLevel = SceneManager.GetActiveScene();
        load = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        load.allowSceneActivation = false;
        onLoadStart.Invoke();

        yield return new WaitUntil(() => load.progress >= 0.9f);

        enterLevelButton.gameObject.SetActive(true);
        enterLevelButton.interactable = true;
        onLoadFinished.Invoke();

        yield return new WaitUntil(() => readyToEnterNewLevel == true);

        load.allowSceneActivation = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        SceneManager.UnloadSceneAsync(oldLevel);
    }

    #endregion









    public IEnumerator SetUpProcessesBeforeActivatingScene(List<GameObject> objectsToRun, Scene newScene, System.Func<bool> criteriaToFinish)
    {
        /*
        GameObject[] objectsInScene = newScene.GetRootGameObjects();
        GameObject tempSceneHolder = new GameObject("Temporary scene holder");
        SceneManager.MoveGameObjectToScene(tempSceneHolder, newScene);
        for (int i = 0; i < objectsInScene.Length; i++)
        {
            if (objectsToRun.Contains(objectsInScene[i]) == false)
            {
                objectsInScene[i].transform.SetParent(tempSceneHolder.transform);
            }
        }
        tempSceneHolder.SetActive(false);

        // Wait until criteria is completed
        yield return new WaitUntil(() => criteriaToFinish.Invoke());

        tempSceneHolder.SetActive(true);
        tempSceneHolder.transform.DetachChildren();
        Destroy(tempSceneHolder);
        */

        /*
        GameObject[] objectsInScene = newScene.GetRootGameObjects();
        bool[] loadedVsUnloaded = new bool[objectsInScene.Length];
        for (int i = 0; i < loadedVsUnloaded.Length; i++)
        {
            loadedVsUnloaded[i] = objectsInScene[i].activeSelf; // Record current active state of each gameobject
            if (objectsToRun.Contains(objectsInScene[i]) == false)
            {
                objectsInScene[i].SetActive(false);
            }
        }

        // Wait until criteria is completed
        yield return new WaitUntil(()=> criteriaToFinish.Invoke());

        for (int i = 0; i < objectsInScene.Length; i++)
        {
            objectsInScene[i].SetActive(loadedVsUnloaded[i]);
        }
        */

        /*
        Scene temporaryScene = SceneManager.CreateScene("Temporary scene");
        for (int i = 0; i < objectsInScene.Length; i++)
        {
            SceneManager.MoveGameObjectToScene(objectsInScene[i], temporaryScene);
        }
        */
        yield return null;
    }








}

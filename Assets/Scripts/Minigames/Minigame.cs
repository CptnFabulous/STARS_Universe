using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class Minigame : MonoBehaviour
{
    
    public TimeValue timeLimit = new TimeValue(0, 0, 30);

    [Header("Effects")]
    public UnityEvent onReset;
    public UnityEvent onStart;
    public UnityEvent onProgress;
    public UnityEvent onWin;
    public UnityEvent onError;
    public UnityEvent onFail;

    public PlayerHandler currentlyPlaying { get; set; }
    public float startTime { get; private set; }

    public virtual void ResetGame()
    {
        currentlyPlaying = null;
        onReset.Invoke();
    }
    public virtual void StartGame(PlayerHandler newPlayer)
    {
        currentlyPlaying = newPlayer;
        onStart.Invoke();
    }
    public virtual void Progress()
    {
        onProgress.Invoke();
    }
    public virtual void Win()
    {
        onWin.Invoke();
    }
    public virtual void Error()
    {
        onError.Invoke();
    }
    public virtual void Fail()
    {
        onFail.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
    }

    public abstract void SetupHUD(MinigameHeadsUpDisplay hud);
    public abstract void UpdateHUD(MinigameHeadsUpDisplay hud);
}

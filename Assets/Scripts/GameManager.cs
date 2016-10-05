using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        MENU,
        OPTIONS,
        INGAME,
        ENDGAME,
        PAUSE,
        CALIBRATING
    };
    
    public Canvas menuUI;
    public Canvas endUI;
    public Canvas inGameUI;
    public Canvas pausedUI;
    public Canvas callibratingUI;
    public Canvas settingsUI;

    public GameState currentState;
    Stack<GameState> stateQueue;
    Canvas currentCanvas = null;

    bool inOptions = false;
    
    PlayerState player;
    GameInput gameInput;

    public event Action InGameEvent;
    public event Action EndGameEvent;

    void Awake()
    {
        endUI.enabled = false;
        pausedUI.enabled = false;
        inGameUI.enabled = false;
        settingsUI.enabled = false;
        callibratingUI.enabled = false;
        menuUI.enabled = false;
        stateQueue = new Stack<GameState>();
        ChangeState(GameState.MENU);
    }

    void Start()
    {
        player = FindObjectOfType<PlayerState>();
        player.playerDeathEvents += OnPlayerDeath;
        gameInput = FindObjectOfType<GameInput>();
    }

    public void Menu()
    {
        if (Screen.sleepTimeout != SleepTimeout.SystemSetting)
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }

    public void Options()
    {
        if (currentState != GameState.OPTIONS) {
            StartCoroutine(OptionsRoutine());
        }
    }

    IEnumerator OptionsRoutine()
    {
        if (Screen.sleepTimeout != SleepTimeout.SystemSetting)
            Screen.sleepTimeout = SleepTimeout.SystemSetting;

        stateQueue.Push(currentState);
        ChangeState(GameState.OPTIONS);
        FindObjectOfType<Settings>().OnShow();
        inOptions = true;
        while (inOptions)
            yield return new WaitForEndOfFrame();
        ChangeState(stateQueue.Pop());
    }

    public void ExitOptions()
    {
        inOptions = false;
    }

    public void StartGame()
    {
        if (Screen.sleepTimeout != SleepTimeout.NeverSleep)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ChangeState(GameState.INGAME);
        Calibrate();
        if (InGameEvent != null)
            InGameEvent();
    }

    public void EndGame()
    {
        if (Screen.sleepTimeout != SleepTimeout.SystemSetting)
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        ChangeState(GameState.ENDGAME);
        if (EndGameEvent != null)
            EndGameEvent();
    }

    public void Pause()
    {
        if (currentState == GameState.INGAME)
        {
            if (Screen.sleepTimeout != SleepTimeout.SystemSetting)
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            ChangeState(GameState.PAUSE);
            Time.timeScale = 0;
        }
    }

    public void Resume()
    {
        if (currentState == GameState.PAUSE)
        {
            if (Screen.sleepTimeout != SleepTimeout.NeverSleep)
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            ChangeState(GameState.INGAME);
            Time.timeScale = 1;
            if (InGameEvent != null)
                InGameEvent();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        Pause();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        if (Screen.sleepTimeout != SleepTimeout.NeverSleep)
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void OnPlayerDeath()
    {
        EndGame();
        player.playerDeathEvents -= OnPlayerDeath;
    }

    public void Calibrate()
    {
        if (currentState != GameState.CALIBRATING)
            StartCoroutine(CalibrateRoutine());
    }

    public IEnumerator CalibrateRoutine()
    {
        stateQueue.Push(currentState);
        ChangeState(GameState.CALIBRATING);
        gameInput.CalibrateAccelerometer();
        while (gameInput.callibrating)
            yield return new WaitForEndOfFrame();
        ChangeState(stateQueue.Pop());
    }

    void ChangeState(GameState state)
    {
        if (currentCanvas != null)
            currentCanvas.enabled = false;

        currentState = state;
        
        switch (currentState)
        {
            case GameState.MENU:
                currentCanvas = menuUI;
                break;
            case GameState.CALIBRATING:
                currentCanvas = callibratingUI;
                break;
            case GameState.INGAME:
                currentCanvas = inGameUI;
                break;
            case GameState.OPTIONS:
                currentCanvas = settingsUI;
                break;
            case GameState.PAUSE:
                currentCanvas = pausedUI;
                break;
            case GameState.ENDGAME:
                currentCanvas = endUI;
                break;
        }

        currentCanvas.enabled = true;
    }

}

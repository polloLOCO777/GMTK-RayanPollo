using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Main Menu")]
    [SerializeField] Canvas mainMenu;
    [SerializeField] GameObject menuButtons;
    [SerializeField] GameObject title;
    [SerializeField] GameObject controls;
    [SerializeField] GameObject credits;

    [Header("Pause")]
    [SerializeField] Canvas pauseMenu;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject settings;

    [Header("Win Menu")]
    [SerializeField] Canvas winMenu;

    [Header("Lose Menu")]
    [SerializeField] Canvas loseMenu;

    [Header("Cameras")]
    [SerializeField] Camera uiCamera;

    public bool IsGamePaused { get; private set; }

    // Rework this to include 'substates' of a certain menuState
    // This would make it easier to add new states, as substates of other states, without haveing to update VirtualCursorActivator, for each state added
    // Plus, they are already unofficially organized like this through the headers
    public enum MenuState { Null, MainMenu, GameStart, Pause, Settings, GameResume, GameEnd, PreviousState, Controls, Credits }

    public MenuState CurrentState { get; private set; }
    public MenuState PreviousState { get; private set; } = MenuState.Null;

    public static event Action<MenuState> OnMenuStateChange;

    void OnEnable()
    {
        OnStateChangeEventHandler += HandleGameStateChange;
    }

    void OnDisable()
    {
        OnStateChangeEventHandler -= HandleGameStateChange;
    }

    private void Update()
    {
        PauseGame();
    }

    //This and the play button kind of do the same thing.
    void HandleGameStateChange(object sender, StateChangeEventArgs e)
    {
        switch (e.newState)
        {
            case GameState.OpenGame:
            break;

            case GameState.StartGame:
            break;
            
            case GameState.StartLevel:
            break;
            
            case GameState.LoseLevel:
            break;
            
            case GameState.RestartLevel:
            break;
            
            case GameState.WinLevel:
            break;
        }
    }

    public void UpdateMenu(MenuState newState)
    {
        if (newState != MenuState.PreviousState)
        {
            PreviousState = CurrentState;
            CurrentState = newState;
        }

        switch (newState)
        {
            case MenuState.MainMenu:
                mainMenu.gameObject.SetActive(true);
                loseMenu.gameObject.SetActive(false);
                menuButtons.SetActive(true);
                title.SetActive(true);

                settings.SetActive(false);
                credits.SetActive(false);
                controls.SetActive(false);

                if (!uiCamera.isActiveAndEnabled)
                {
                    Debug.LogWarning("MenuCamera was not active. Setting Cam active");
                    uiCamera.enabled = true;
                }
                break;

            case MenuState.GameStart:
                uiCamera.enabled = false;

                mainMenu.gameObject.SetActive(false);
            break;

            case MenuState.Pause:
                IsGamePaused = true;

                uiCamera.enabled = true;

                controlsPanel.SetActive(true);
                pauseMenu.gameObject.SetActive(true);

                menuButtons.SetActive(false);
                settings.SetActive(false);
            break;

            case MenuState.Settings:
                menuButtons.SetActive(false);
                controlsPanel.SetActive(false);
                title.SetActive(false);

                settings.SetActive(true);
            break;

            case MenuState.Credits:
                credits.SetActive(true);
            break;

            case MenuState.Controls:
                controls.SetActive(true);
            break;

            case MenuState.GameResume:
                IsGamePaused = false;

                uiCamera.enabled = false;

                pauseMenu.gameObject.SetActive(false);
            break;

            case MenuState.GameEnd:
                uiCamera.enabled = true;

                loseMenu.gameObject.SetActive(true);
            break;

            case MenuState.PreviousState:
                UpdateMenu(PreviousState);
            break;

            default:
                Debug.Log("Unknown Menu State");
            break;
        }

        OnMenuStateChange.Invoke(newState);
    }

    /// <summary>
    ///     Puase and unpause game on escape press.
    /// </summary>
    void PauseGame()
    {
        var keyPress = KeyCode.Escape;

#if DEBUG 
        keyPress = KeyCode.P;
#endif

        if (Input.GetKeyDown(keyPress))
        {
            switch (CurrentState)
            {
                case MenuState.Pause:
                    UpdateMenu(MenuState.GameResume);
                    break;

                case MenuState.GameStart:
                case MenuState.GameResume:
                    UpdateMenu(MenuState.Pause);
                    break;
            }
        }
    }
}
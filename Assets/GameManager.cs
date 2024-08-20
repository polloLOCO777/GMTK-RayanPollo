using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { StartGame, StartLevel, LoseLevel, RestartLevel, WinLevel, }

    public static event EventHandler<StateChangeEventArgs> OnStateChangeEventHandler;

    public GameState CurrentState { get; private set; }

    public class StateChangeEventArgs : EventArgs
    {
        public readonly GameState newState;
        public readonly GameState previousState;
        public readonly int levelToLoad;

        public StateChangeEventArgs(GameState _newState, GameState _previousState, int _levelToLoad = -1)
        {
            newState = _newState;
            previousState = _previousState;
            levelToLoad = _levelToLoad;
        }
    }

    private void OnEnable()
    {
        Goal.OnReachGoalEventHandler += HandleReachGoal;
    }

    private void OnDisable()
    {
        Goal.OnReachGoalEventHandler -= HandleReachGoal;
    }

    void Start()
    {
        UpdateGameState(GameState.StartGame);
    }

    void HandleReachGoal(object sender, EventArgs e)
    {
        UpdateGameState(GameState.WinLevel);
    }

    /// <summary>
    ///     Informs listeners on how to align with the current state of the game.
    /// </summary>
    /// <param name="newState"> The state of the game to update to. </param>
    public void UpdateGameState(GameState newState)
    {
        var previousState = CurrentState;
        CurrentState = newState;

        OnStateChangeEventHandler?.Invoke(this, new (newState, previousState));
    }
}
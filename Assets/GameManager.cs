using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { LevelStart, LevelLose, LevelRestart, LevelWin, }

    public static event EventHandler<StateChangeEventArgs> OnStateChangeEventHandler;
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

    public GameState CurrentState { get; private set; }

    /// <summary>
    ///     Informs listeners on how to align with the current state of the game.
    /// </summary>
    /// <param name="newState"> The state of the game to update to. </param>
    public void UpdateGameState(GameState newState)
    {
        var previousState = CurrentState;
        CurrentState = newState;

        OnStateChange(new(newState, previousState));
    }

    void OnStateChange(StateChangeEventArgs e)
        => OnStateChangeEventHandler?.Invoke(this, e);
}
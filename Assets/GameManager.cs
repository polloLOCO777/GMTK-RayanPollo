using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { OpenGame, StartLevel, LoseLevel, RestartLevel, BeatLevel, BeatGame }

    public static event EventHandler<StateChangeEventArgs> OnGameStateChangeEventHandler;

    public GameState CurrentState { get; private set; }

    public class StateChangeEventArgs : EventArgs
    {
        public readonly GameState newState;
        public readonly GameState previousState;
        public readonly int levelToLoad;

        public StateChangeEventArgs(GameState _newState, GameState _previousState, int _levelToLoad)
        {
            newState = _newState;
            previousState = _previousState;
            levelToLoad = _levelToLoad;
        }
    }

    private void OnEnable()
    {
        Goal.OnReachGoalEventHandler += HandleReachGoal;
        ConsumePlayer.LoseLevelEventHandler += HandleLoseLevel;
        LevelManager.OnBeatLastLevelEventHandler += HandleBeatLastLevel;
    }
    private void OnDisable()
    {
        Goal.OnReachGoalEventHandler -= HandleReachGoal;
        ConsumePlayer.LoseLevelEventHandler -= HandleLoseLevel;
        LevelManager.OnBeatLastLevelEventHandler -= HandleBeatLastLevel;
    }

    void HandleReachGoal(object sender, EventArgs e)
        => UpdateGameState(GameState.BeatLevel);

    void HandleLoseLevel(object sender, EventArgs e)
        => UpdateGameState(GameState.RestartLevel);

    void HandleBeatLastLevel(object sender, EventArgs e)
        => UpdateGameState(GameState.OpenGame); 

    void Start()
    {
        bool foundTestLevel = false;

#if DEBUG
        // Starts the game by unloading and reloading the level already in the scene
        int levelToLoad = -1;
        string levelString;
        string levelToUnload = string.Empty;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if (scene.name[..5] != "Level")
                continue;

            levelString = scene.name[(scene.name.LastIndexOf('_') + 1)..];

            if (int.TryParse(levelString, out int levelNumber))
            {
                levelToUnload = scene.name;
                levelToLoad = levelNumber;
            }
        }

        if (LevelManager.DoesLevelExist(levelToLoad))
        {
            foundTestLevel = true;
            Debug.Log($"TestLevel: {levelToLoad}");

            if (!string.IsNullOrEmpty(levelToUnload))
                SceneManager.UnloadSceneAsync(levelToUnload);
            
            UpdateGameState(GameState.StartLevel, levelToLoad);
        }
#endif
        if (!foundTestLevel)
            UpdateGameState(GameState.OpenGame);
    }

    /// <summary>
    ///     Informs listeners on how to align with the current state of the game.
    /// </summary>
    /// <param name="newState"> The state of the game to update to. </param>
    public void UpdateGameState(GameState newState, int levelToLoad = -1)
    {
        var previousState = CurrentState;
        CurrentState = newState;

        OnGameStateChangeEventHandler?.Invoke(this, new (newState, previousState, levelToLoad));
    }
}
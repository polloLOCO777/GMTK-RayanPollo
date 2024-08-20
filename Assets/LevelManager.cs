using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] string levelConvention = "Level_";
    public class LevelLoadStartEventArgs : EventArgs
    {
        public readonly AsyncOperation asyncOperation;

        public LevelLoadStartEventArgs(AsyncOperation _asyncOperation)
            => asyncOperation = _asyncOperation;
    }

    public int CurrentLevel { get; private set; }

    string lastLoadedLevel = null;

    public static event EventHandler<LevelLoadStartEventArgs> OnStartLevelLoadEventHandler;
    public static event EventHandler OnBeatLastLevelEventHandler;

    private void OnEnable()
    {
        GameManager.OnGameStateChangeEventHandler += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChangeEventHandler -= HandleGameStateChange;
    }

    /// <summary>
    ///     Handles scene and level loading for various game updates.
    /// </summary>
    /// <exception cref="ArgumentException"> Exception on invalid level number when loading a level. </exception>
    public void HandleGameStateChange(object sender, GameManager.StateChangeEventArgs e)
    {
        switch (e.newState)
        {
            case GameManager.GameState.OpenGame:
                LoadLevel(1);
            break;

            case GameManager.GameState.StartLevel:
                
                if (e.levelToLoad == -1)
                    throw new ArgumentException("Level to load should not be -1. ");
                LoadLevel(e.levelToLoad);
            break;
            
            case GameManager.GameState.LoseLevel:
            case GameManager.GameState.RestartLevel:
                LoadLevel(CurrentLevel);
            break;
            
            case GameManager.GameState.BeatLevel:
                if (!LoadLevel(CurrentLevel + 1))
                    OnBeatLastLevel();
            break;
        }
    }

    /// <summary>
    ///     Asyrnchously loads a level and unloads the previous level.
    /// </summary>
    /// <param name="level"> The number of the level to unload. </param>
    /// <returns> True if level is found. </returns>
    public bool LoadLevel(int level)
    {
        UnloadScene(lastLoadedLevel);
        lastLoadedLevel = $"{levelConvention}{level}";
        CurrentLevel = level;
        return LoadScene($"{levelConvention}{level}");
    }

    /// <summary>
    ///     Asynchronously loads a scene.
    /// </summary>
    /// <param name="sceneName"> The name of the scene to load. </param>
    /// <returns> True if the scene starts loading. </returns>
    bool LoadScene(string sceneName)
    {
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
            return false;

        AsyncOperation levelLoadAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        OnStartLevelLoad(levelLoadAsyncOperation);
        return true;
    }

    /// <summary>
    ///     Called when we begin asynchronously loading a level.
    /// </summary>
    /// <param name="levelLoadAsyncOperation"> The level loading operation. </param>
    public void OnStartLevelLoad(AsyncOperation levelLoadAsyncOperation)
    {
        LevelLoadStartEventArgs eventArgs = new(levelLoadAsyncOperation);
        OnStartLevelLoadEventHandler?.Invoke(this, eventArgs);
    }

    public void OnBeatLastLevel()
        => OnBeatLastLevelEventHandler?.Invoke(this, new());

    /// <summary>
    ///     Checks if a level is in the build path.
    /// </summary>
    /// <param name="levelnumber"> The nummber of the level to check. </param>
    /// <returns> True if a scene is successfully found. </returns>
    public static bool DoesLevelExist(int levelnumber)
    {
        if (Instance == null)
            return false;

        return SceneUtility.GetBuildIndexByScenePath($"{Instance.levelConvention}{levelnumber}") != -1;
    }

    /// <summary>
    ///     Asynchronously unloads a scene.
    /// </summary>
    /// <param name="sceneName"> The name of the scene to unload. </param>
    /// <returns> True if the scene is succesfully unloaded. </returns>
    public bool UnloadScene(string sceneName)
    {
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
            return false;

        if (lastLoadedLevel == null)
            return false;

        SceneManager.UnloadSceneAsync(sceneName);

        return true;
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] string levelConvention = "Level_";

    public static event EventHandler<LevelLoadStartEventArgs> OnStartSceneLoadEventHandler;

    public class LevelLoadStartEventArgs : EventArgs
    {
        public readonly AsyncOperation asyncOperation;

        public LevelLoadStartEventArgs(AsyncOperation _asyncOperation)
        {
            asyncOperation = _asyncOperation;
        }
    }

    public int CurrentLevel { get; private set; }

    string lastLoadedLevel = null;

    /// <summary>
    ///     Handles scene and level loading for various game updates.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void HandleGameStateChange(object sender, GameManager.StateChangeEventArgs e)
    {
        switch (e.newState)
        {
            case GameManager.GameState.LevelStart:
                if (e.levelToLoad == -1)
                    throw new NotImplementedException();

                CurrentLevel = e.levelToLoad;

                LoadLevel(e.levelToLoad);
            break;
            
            case GameManager.GameState.LevelLose:
            case GameManager.GameState.LevelRestart:
                LoadLevel(CurrentLevel);
            break;
            
            case GameManager.GameState.LevelWin:
                LoadLevel(CurrentLevel + 1);
            break;
        }
    }

    /// <summary>
    ///     Asyrnchously loads a level and unloads the previous level.
    /// </summary>
    /// <param name="level"> The number of the level to unload. </param>
    /// <returns> True if the level starts loading. </returns>
    public bool LoadLevel(int level)
    {
        UnloadScene(lastLoadedLevel);
        lastLoadedLevel = $"{levelConvention}{level}";
        return LoadScene($"{levelConvention}{level}");
    }

    /// <summary>
    ///     Asynchronously loads a scene.
    /// </summary>
    /// <param name="sceneName"> The name of the scene to load. </param>
    /// <returns> True if the scene starts loading. </returns>
    public bool LoadScene(string sceneName)
    {
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
            return false;

        if (!GameManager.Instance)
            Debug.Log("No Game Manager Instance");

        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        OnStartLevelLoad(new(load));
        return true;
    }

    public void OnStartLevelLoad(LevelLoadStartEventArgs e)
        => OnStartSceneLoadEventHandler?.Invoke(this, e);

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
    /// <param name="bypass"></param>
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
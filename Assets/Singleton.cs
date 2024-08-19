using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides a publicly available static instance of the class.
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// Ensures only one instance of the class exists at a time.
/// </summary>
public abstract class SingleInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    static T Instance { get; set; }

    protected virtual void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        Instance = this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}

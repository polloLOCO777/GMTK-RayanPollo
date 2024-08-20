using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Goal : MonoBehaviour
{
    public static event EventHandler OnReachGoalEventHandler;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            OnReachGoal();
    }

    void OnReachGoal()
        => OnReachGoalEventHandler?.Invoke(this, new());
}

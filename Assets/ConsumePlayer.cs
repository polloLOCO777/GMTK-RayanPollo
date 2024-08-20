using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumePlayer : MonoBehaviour
{
    public static event EventHandler LoseLevelEventHandler;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Kill player");
            LoseLevelEventHandler?.Invoke(this, new());
            gameObject.SetActive(false);
        }
    }
}

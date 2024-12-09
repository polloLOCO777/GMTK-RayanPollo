using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("GameObject")]
    private Transform Player;

    [Header("Properties")] 
    public float speed;

    [Header("Script")]
    public Detector detector;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("GameController").transform;
    }

    private void FixedUpdate()
    {
        if (detector == true) 
        {
            transform.position = Vector2.MoveTowards(transform.position, Player.position, speed * Time.deltaTime);
        }
    }

}

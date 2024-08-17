using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;

    public float Speed;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigidBody2D.rotation += Speed;
    }
}

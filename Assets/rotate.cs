using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rotate : MonoBehaviour
{
    [Header("Properties")]
    [FormerlySerializedAs("Speed")]
    [SerializeField] float speed;

    [Header("Components")]
    [SerializeField] Rigidbody2D rigidBody2D;

    void FixedUpdate() 
        => rigidBody2D.rotation += speed;
}

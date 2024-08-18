using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGone : MonoBehaviour
{
    public float potencia;
    public Transform blackHole;
    public float Timer;
    public float TimeToGo;
    public Rigidbody2D rigidBody2D;

    public float Speed;


    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        blackHole = GameObject.FindGameObjectWithTag("Destroyer").transform;
    }

    private void FixedUpdate()
    {
        Timer += Time.deltaTime;
        if (Timer > TimeToGo)
        {
            transform.position = Vector2.Lerp(transform.position, blackHole.position, potencia * Time.deltaTime);
            transform.localScale -= new Vector3(0.02f, 0.02f);
            rigidBody2D.rotation += Speed;
        }
    }

    void Update()
    {
        if (transform.localScale == new Vector3(0f, 0f, 1f))
        {
            CineMachineMovimientoCamara.Instance.MoverCamara(1, 1, 0.5f);
            BlackHole.Instance.More();
            Destroy(gameObject);
        }

            

    }
}

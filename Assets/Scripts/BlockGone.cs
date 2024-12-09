using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGone : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float power = 3; // potencia
    [SerializeField] float speed = 8;
    [SerializeField] float lifeTime = 1;

    [Header("Components")]
    [SerializeField] Rigidbody2D rigidBody2D;

    Transform blackHole;
    float timer;

    public static event EventHandler<ProxyDisappearEventArgs> OnProxyDisappearEventHandler;

    public class ProxyDisappearEventArgs : EventArgs { }

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        blackHole = GameObject.FindGameObjectWithTag("Destroyer").transform;
    }

    /// <summary>
    ///     Shrinks and moves towards the black hole over time.
    /// </summary>
    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if (timer <= lifeTime)
            return;

        transform.position = Vector2.Lerp(transform.position, blackHole.position, power * Time.deltaTime);
        transform.localScale -= new Vector3(0.02f, 0.02f);
        rigidBody2D.rotation += speed;
    }

    /// <summary>
    ///     Increases the black hole size after disappearing.
    /// </summary>
    void Update()
    {
        if (transform.localScale != new Vector3(0f, 0f, 1f))
            return;

        OnProxyDisappear(new());
        Destroy(gameObject);
    }

    void OnProxyDisappear(ProxyDisappearEventArgs e)
        => OnProxyDisappearEventHandler?.Invoke(this, e);
}

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

    public static event EventHandler<ProxyEventArgs> OnProxyEventHandler;

    public class ProxyEventArgs : EventArgs 
    { 
        public enum ActionType { Appear, ShrinkStart, Disappear }

        public readonly ActionType action;

        public ProxyEventArgs(ActionType _action)
        {
            action = _action;
        }
    }

    bool hasStartedShrinking;

    private void OnEnable()
        => hasStartedShrinking = false;

    void Start()
    {
        OnProxy(new(ProxyEventArgs.ActionType.Appear));
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

        if (!hasStartedShrinking)
        {
            OnProxy(new(ProxyEventArgs.ActionType.ShrinkStart));
            hasStartedShrinking = true;
        }

        transform.position = Vector2.Lerp(transform.position, blackHole.position, power * Time.fixedDeltaTime);
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

        OnProxy(new(ProxyEventArgs.ActionType.Disappear));
        Destroy(gameObject);
    }

    void OnProxy(ProxyEventArgs e)
        => OnProxyEventHandler?.Invoke(this, e);
}

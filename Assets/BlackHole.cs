using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] float timeToAbsorb = 5;
    [SerializeField] Transform part;

    public static event EventHandler<TugTileEventArgs> OnTugTileEventHandler;
    public static event EventHandler<ConsumeEventArgs> OnConsumeEventHandler;
    
    float timer;

    public class TugTileEventArgs { }
    public class ConsumeEventArgs 
    {
        public enum ObjectType { Tile }

        public readonly ObjectType objectType;
        public readonly GameObject gameObject;
        public readonly Collider2D collider;
        public readonly Transform transform;

        public ConsumeEventArgs(ObjectType _objectType, Collider2D _collider)
        {
            collider = _collider;
            objectType = _objectType;
            gameObject = _collider.gameObject;
            transform = _collider.transform;
        }
    }

    private void OnEnable()
        => BlockGone.OnProxyDisappearEventHandler += HandleProxyDisappear;

    private void OnDisable()
        => BlockGone.OnProxyDisappearEventHandler -= HandleProxyDisappear;

    /// <summary>
    ///     Informs listeners we're ready to pull in a tile.
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeToAbsorb)
        {
            timer = 0;
            OnTugTile(new());
        }
    }

    /// <summary>
    ///     Consumes blocks that get too close to the black hole.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Block":
                OnConsume(new(ConsumeEventArgs.ObjectType.Tile, collision));
            break;
        }
    }

    /// <summary>
    ///     Raised every n seconds to begin pulling in a tile.
    /// </summary>
    void OnTugTile(TugTileEventArgs e)
        => OnTugTileEventHandler?.Invoke(this, e);

    /// <summary>
    ///     Raised when a tile is too close to the black hole. 
    /// </summary>
    /// <param name="e"></param>
    void OnConsume(ConsumeEventArgs e)
        => OnConsumeEventHandler?.Invoke(this, e);

    /// <summary>
    ///     Increase the size of the black hole.
    /// </summary>
    public void Bigger()
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        part.localScale += new Vector3(0.1f, 0.1f);
    }

    /// <summary>
    ///     <para>
    ///         After pulling in a block:                   <br/>
    ///         1. Increase the pull of the black hole.     <br/>
    ///         2. Increase the size of the black hole.
    ///     </para>
    /// </summary>
    void HandleProxyDisappear(object sender, BlockGone.ProxyDisappearEventArgs e)
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        part.localScale += new Vector3(0.1f, 0.1f);

        timeToAbsorb -= .2f;

        Invoke(nameof(Bigger), 0.05f);
        Invoke(nameof(Bigger), 0.1f);
        Invoke(nameof(Bigger), 0.15f);
    }
 }

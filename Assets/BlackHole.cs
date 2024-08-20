using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] List<float> growthTargets = new();
    [SerializeField] float timeUntilGrowthCheck;
    [SerializeField] float timeToAbsorb = 5;
    [SerializeField] Transform part;

    public static event EventHandler<ConsumeEventArgs> OnConsumeEventHandler;
    public static event EventHandler OnTugTileEventHandler;
    public static event EventHandler OnGrowEventHandler;

    float tugTileTimer;

    int growthTargetIndex;

    public class BlackHoleEventArgs { }
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
        => BlockGone.OnProxyEventHandler += HandleProxyAction;
    
    private void OnDisable()
        => BlockGone.OnProxyEventHandler -= HandleProxyAction;

    /// <summary>
    ///     Informs listeners we're ready to pull in a tile.
    /// </summary>
    private void Update()
    {
        tugTileTimer += Time.deltaTime;

        if (tugTileTimer > timeToAbsorb)
        {
            tugTileTimer = 0;
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
    ///     Increase the size of the black hole.
    /// </summary>
    public void Bigger()
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        part.localScale += new Vector3(0.1f, 0.1f);

        if (growthTargetIndex >= growthTargets.Count)
            return;

        if (transform.localScale.x >= growthTargets[growthTargetIndex])
        {
            growthTargetIndex++;
            OnGrow(new());
        }
    }

    /// <summary>
    ///     <para>
    ///         After pulling in a block:                   <br/>
    ///         1. Increase the pull of the black hole.     <br/>
    ///         2. Increase the size of the black hole.
    ///     </para>
    /// </summary>
    void HandleProxyAction(object sender, BlockGone.ProxyEventArgs e)
    {
        if (e.action != BlockGone.ProxyEventArgs.ActionType.Disappear)
            return;

        transform.localScale += new Vector3(0.1f, 0.1f);
        part.localScale += new Vector3(0.1f, 0.1f);

        timeToAbsorb -= .2f;

        Invoke(nameof(Bigger), 0.05f);
        Invoke(nameof(Bigger), 0.1f);
        Invoke(nameof(Bigger), 0.15f);
    }

    /// <summary>
    ///     Raised every n seconds to begin pulling in a tile.
    /// </summary>
    void OnTugTile(EventArgs e)
        => OnTugTileEventHandler?.Invoke(this, e);

    /// <summary>
    ///     Called whenever the black hole grows in size
    /// </summary>
    /// <param name="e"></param>
    void OnGrow(EventArgs e)
        => OnGrowEventHandler?.Invoke(this, e);

    /// <summary>
    ///     Raised when a tile is too close to the black hole. 
    /// </summary>
    /// <param name="e"></param>
    void OnConsume(ConsumeEventArgs e)
        => OnConsumeEventHandler?.Invoke(this, e);
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.ParticleSystem;

public class BlackHole : MonoBehaviour
{
    [SerializeField] Transform part;

    [SerializeField] BlockGone blockGone;

    public static BlackHole Instance;

    private void OnEnable()
        => BlockGone.OnBlockDisappearEventHandler += HandleBlockDisappear;

    private void OnDisable()
        => BlockGone.OnBlockDisappearEventHandler -= HandleBlockDisappear;

    private void Awake()
        => Instance = this;

    private void Update()
    {
        if (transform.localScale != new Vector3(0f, 0f, 1f))
            return;
    }

    /// <summary>
    ///     Consumes blocks that get too close to the black hole.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Block":
                Instantiate(blockGone, collision.transform.position, collision.transform.rotation); // Move this into different script
                collision.transform.SetParent(Map.Instance.InactiveTilesParent);
                collision.gameObject.SetActive(false);
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
    }

    /// <summary>
    ///     <para>
    ///         After pulling in a block:                   <br/>
    ///         1. Increase the pull of the black hole.     <br/>
    ///         2. Increase the size of the black hole.
    ///     </para>
    /// </summary>
    void HandleBlockDisappear(object sender, BlockGone.BlockDisappearEventArgs e)
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        part.localScale += new Vector3(0.1f, 0.1f);

        Map.Instance.SetTimeToAbsorb(Map.Instance.GetTimeToAbsorb() - .2f);

        Invoke(nameof(Bigger), 0.05f);
        Invoke(nameof(Bigger), 0.1f);
        Invoke(nameof(Bigger), 0.15f);
    }
 }

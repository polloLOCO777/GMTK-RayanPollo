using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.ParticleSystem;

public class BlackHole : MonoBehaviour
{
    public static BlackHole Instance;
    [SerializeField] Transform part;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    ///     <para>
    ///         After pulling in a block:                   <br/>
    ///         1. Increase the pull of the black hole.     <br/>
    ///         2. Increase the size of the black hole.
    ///     </para>
    /// </summary>
    public void More()
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        part.localScale += new Vector3(0.1f, 0.1f);

        Atractor.Instance.SetRadius(Atractor.Instance.GetRadius() + .5f);
        Atractor.Instance.SetGravity(Atractor.Instance.GetRadius() - 1);

        Map.Instance.SetTimeToAbsorb(Map.Instance.GetTimeToAbsorb() - .2f);

        Invoke(nameof(Bigger), 0.05f);
        Invoke(nameof(Bigger), 0.1f);
        Invoke(nameof(Bigger), 0.15f);
    }

    /// <summary>
    ///     Increase the size of the black hole.
    /// </summary>
    public void Bigger()
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        part.localScale += new Vector3(0.1f, 0.1f);
    }
 }

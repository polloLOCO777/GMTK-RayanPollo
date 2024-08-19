using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public static BlackHole Instance;
    public Transform Part;

    private void Awake()
    {
        Instance = this;
    }

    public void More()
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        Part.localScale += new Vector3(0.1f, 0.1f);

        Atractor.Instance.SetRadius(Atractor.Instance.GetRadius() + .5f);
        Atractor.Instance.SetGravity(Atractor.Instance.GetRadius() - 1);

        Map.Instance.SetTimeToAbsorb(Map.Instance.GetTimeToAbsorb() - .2f);

        Invoke(nameof(Bigger), 0.15f);
        Invoke(nameof(Bigger), 0.05f);
        Invoke(nameof(Bigger), 0.1f);
    }

    public void Bigger()
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        Part.localScale += new Vector3(0.1f, 0.1f);
    }
 }

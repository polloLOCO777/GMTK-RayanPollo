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
        Atractor.Instance.Radius += 0.5f;
        Atractor.Instance.gravity -= 1f;
        Map.Instance.TimeToAbsorb -= 0.2f;
        Invoke("Bigger", 0.15f);
        Invoke("Bigger", 0.05f);
        Invoke("Bigger", 0.1f);
    }

    public void Bigger()
    {
        transform.localScale += new Vector3(0.1f, 0.1f);
        Part.localScale += new Vector3(0.1f, 0.1f);
    }
 }

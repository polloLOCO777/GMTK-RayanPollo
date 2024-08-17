using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float TimeToDestroy;
    void Start()
    {
        Destroy(gameObject, TimeToDestroy);
    }

}

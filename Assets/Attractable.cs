using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attractable : MonoBehaviour
{
    [field: SerializeField] public Collider2D Collider { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }

    bool isBeingPulled;

    public void SetIsBeingPulled(bool value)
        => isBeingPulled = value;

    private void Update()
    {
        if (isBeingPulled)
            Rigidbody.gravityScale = 0;
        else
            Rigidbody.gravityScale = 2;
    }
}

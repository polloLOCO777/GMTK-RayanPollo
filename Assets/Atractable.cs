using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atractable : MonoBehaviour
{
    [SerializeField] bool rotateToCenter = true;
    [SerializeField] float gravityStrength = 100;
    [SerializeField] Atractor currentAttractor;

    new Collider2D collider;
    Rigidbody2D rigdibody;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        rigdibody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (currentAttractor == null)
        {
            rigdibody.gravityScale = 2;
            return;
        }

        if (!currentAttractor.AttractedObjects.Contains(collider))
        {
            currentAttractor = null;
            return;
        }

        if (rotateToCenter) RotateToCenter();
            rigdibody.gravityScale = 0;
    }

    public void Attract(Atractor attractorObj)
    {
        Vector2 attractionDir = ((Vector2)attractorObj.AttractorTransform.position - rigdibody.position).normalized;
        rigdibody.AddForce(-attractorObj.GetGravity() * gravityStrength * Time.fixedDeltaTime * attractionDir);

        if (currentAttractor == null)
            currentAttractor = attractorObj;
    }

    void RotateToCenter()
    {
        if (currentAttractor == null)
            return;

        Vector2 distanceVector = (Vector2)currentAttractor.AttractorTransform.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
}

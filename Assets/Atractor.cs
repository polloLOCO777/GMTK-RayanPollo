using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Name should be: "Attractor"
public class Atractor : MonoBehaviour
{
    // Variable names should start with a lowercase letter
    // SerializeField: https://docs.unity3d.com/es/current/ScriptReference/SerializeField.html
    [field: SerializeField] LayerMask attractionLayer;

    // Properties begin with an uppercase letter
    // Properties: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties
    [SerializeField] float gravity = 10;
    [SerializeField] float radius = 10;

    public float GetGravity() => gravity;
    public float GetRadius() => radius;
    public void SetGravity(float value) => gravity = value;
    public void SetRadius(float value) => radius = value;


    [SerializeField] public List<Collider2D> AttractedObjects = new();

    public Transform AttractorTransform { get; private set; }

    public static Atractor Instance;


    void Awake()
    {
        Instance = this;
        AttractorTransform = GetComponent<Transform>();
    }

    void Update()
    {
        SetAttractedObjects();
    }

    void FixedUpdate()
    {
        AttractObjects();
    }

    void SetAttractedObjects()
    {
        AttractedObjects = Physics2D.OverlapCircleAll(AttractorTransform.position, radius, attractionLayer).ToList();
    }

    void AttractObjects()
    {
        for (int i = 0; i < AttractedObjects.Count; i++)
        {
            AttractedObjects[i].GetComponent<Atractable>().Attract(this);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}

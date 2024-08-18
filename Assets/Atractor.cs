using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Atractor : MonoBehaviour
{
    public LayerMask AttractionLayer;
    public float gravity = 10;
    public float Radius = 10;
    public List<Collider2D> AttractedObjects = new List<Collider2D>();
    [HideInInspector] public Transform attractorTransform;

    public static Atractor Instance;


    void Awake()
    {
        Instance = this;
        attractorTransform = GetComponent<Transform>();
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
        AttractedObjects = Physics2D.OverlapCircleAll(attractorTransform.position, Radius, AttractionLayer).ToList();
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
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

}

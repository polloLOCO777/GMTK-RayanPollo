using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    [SerializeField] float startingGravity = 10;
    [SerializeField] float startingRadius = 10;
    [SerializeField] float gravityMultiplier = 100;
    [SerializeField] float gravityIncreasePerTile = 1;
    [SerializeField] float radiusIncreasePerTile = .5f;
    [SerializeField] LayerMask attractionLayer;

    float gravity;
    float radius;
    public float GetRadius() => radius;
    public void SetGravity(float value) => gravity = value;
    public void SetRadius(float value) => radius = value;

    public List<Attractable> NearbyAttractableObjectys { get; private set; } = new();

    private void Start()
    {
        gravity = startingGravity;
        radius = startingRadius;
    }

    void OnEnable()
        => BlockGone.OnProxyEventHandler += HandleBlockDisappear;

    private void OnDisable()
        => BlockGone.OnProxyEventHandler -= HandleBlockDisappear;

    /// <summary>
    ///     Pulls in nearby attractable objects.
    /// </summary>
    void Update()
    {
        FindNearybyAttractableObjects();

        foreach (Attractable nearbyObject in NearbyAttractableObjectys)
        {
            nearbyObject.SetIsBeingPulled(true);

            // Pull objects
            Vector2 attractionDir = ((Vector2)transform.position - nearbyObject.Rigidbody.position).normalized;
            nearbyObject.Rigidbody.AddForce(gravity * gravityMultiplier * Time.deltaTime * attractionDir);

            // Rotate objects
            Vector2 distanceVector = (Vector2)transform.position - (Vector2)nearbyObject.transform.position;
            float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
            nearbyObject.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        }
    }

    /// <summary>
    ///     Fills list with nearby objects that have the Attractable component.
    /// </summary>
    void FindNearybyAttractableObjects()
    {
        foreach (var obj in NearbyAttractableObjectys)
            obj.SetIsBeingPulled(false);
        NearbyAttractableObjectys.Clear();

        var nearbyObjects = Physics2D.OverlapCircleAll(transform.position, radius, attractionLayer).ToList();
        foreach (var obj in nearbyObjects)
        {
            if (obj.TryGetComponent<Attractable>(out var attractable) && attractable.enabled)
                NearbyAttractableObjectys.Add(attractable);
        }
    }

    // This should not affect all attractors universally, because it inheritly ties all attractors to black holes and all attractors to each other
    /// <summary>
    ///     Increases pull strength and pull radius for all attractors, when any black hole eats a tile.
    /// </summary>
    void HandleBlockDisappear(object sender, BlockGone.ProxyEventArgs e)
    {
        radius += radiusIncreasePerTile;
        gravity += gravityIncreasePerTile;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}

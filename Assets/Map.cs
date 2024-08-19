using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Map : MonoBehaviour
{
    [SerializeField] float timeToAbsorb = 5;
    [FormerlySerializedAs("Remplasar")]
    [SerializeField] GameObject remplasar; // replacement

    [SerializeField] Transform activeTilesParent;
    [field: SerializeField] public Transform InactiveTilesParent { get; private set; }
    public static Map Instance { get; private set; }
    
    float timer;

    public float GetTimeToAbsorb() => timeToAbsorb;
    public float SetTimeToAbsorb(float value) => timeToAbsorb = value;

    private void Awake()
        => Instance = this;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeToAbsorb)
        {
            timer = 0;
            DisableRandomTile();
        }
    }

    /// <summary>
    ///     Chooses a random tile to pull into the black hole.
    /// </summary>
    void DisableRandomTile()
    {
        int childCount = activeTilesParent.childCount;
        if (childCount == 0) 
            return;

        int random = Random.Range(0, childCount);
        GameObject randomTile = activeTilesParent.GetChild(random).gameObject;

        // Disable tile and create proxy
        Instantiate(remplasar, randomTile.transform.position, randomTile.transform.rotation);
        randomTile.SetActive(false);
        randomTile.transform.SetParent(InactiveTilesParent);
    }
}

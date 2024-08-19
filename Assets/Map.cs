using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Map : MonoBehaviour
{
    [SerializeField] float timeToAbsorb = 5;
    [SerializeField] GameObject Remplasar; // replace

    [SerializeField] Transform activeTilesParent;
    [field: SerializeField] public Transform InactiveTilesParent { get; private set; }

    float timer;
    public static Map Instance { get; private set; }

    public float GetTimeToAbsorb() => timeToAbsorb;
    public float SetTimeToAbsorb(float value) => timeToAbsorb = value;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }

    private void Update()
    {
        if (timer <= timeToAbsorb)
            return;

        timer = 0;
        PullInRandomTile();
    }

    /// <summary>
    ///     Chooses a non-null random tile from the map to pull into the black hole.
    /// </summary>
    void PullInRandomTile()
    {
        int childCount = activeTilesParent.childCount;
        if (childCount == 0) 
            return;

        int randomIndex = Random.Range(0, childCount);
        GameObject randomTile = activeTilesParent.GetChild(randomIndex).gameObject;

        PullInTile(randomTile);
    }

    /// <summary>
    ///     Pulls a given tile into the black hole.
    /// </summary>
    /// <param name="tile">
    ///     Tile to pull in.
    /// </param>
    void PullInTile(GameObject tile)
    {
        Instantiate(Remplasar, tile.transform.position, tile.transform.rotation);
        tile.SetActive(false);
        tile.transform.SetParent(InactiveTilesParent);
    }
}

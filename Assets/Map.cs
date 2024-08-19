using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Map : MonoBehaviour
{
    [FormerlySerializedAs("Remplasar")]
    [SerializeField] BlockGone blockProxy; // remplasar

    [SerializeField] Transform activeTilesParent;
    [field: SerializeField] public Transform InactiveTilesParent { get; private set; }

    private void OnEnable()
    {
        BlackHole.OnConsumeEventHandler += HandleConsume;
        BlackHole.OnTugTileEventHandler += HandlePullRandomTile;
    } 

    private void OnDisable()
    {
        BlackHole.OnConsumeEventHandler -= HandleConsume;
        BlackHole.OnTugTileEventHandler -= HandlePullRandomTile;
    } 

    /// <summary>
    ///     Disables a random tile and creates a tile proxy.
    /// </summary>
    void HandlePullRandomTile(object sender, BlackHole.TugTileEventArgs e)
    {
        int childCount = activeTilesParent.childCount;
        if (childCount == 0) 
            return;

        int random = Random.Range(0, childCount);
        GameObject randomTile = activeTilesParent.GetChild(random).gameObject;

        Instantiate(blockProxy, randomTile.transform.position, randomTile.transform.rotation);
        randomTile.SetActive(false);
        randomTile.transform.SetParent(InactiveTilesParent);
    }

    /// <summary>
    ///     Pulls tiles nearby the black hole into the black hole.
    /// </summary>
    /// <param name="e">Details about the object being consumed.</param>
    void HandleConsume(object sender, BlackHole.ConsumeEventArgs e)
    {
        if (e.objectType != BlackHole.ConsumeEventArgs.ObjectType.Tile)
            return;

        Instantiate(blockProxy, e.transform.position, e.transform.rotation);
        e.transform.SetParent(InactiveTilesParent);
        e.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Map : MonoBehaviour
{
    [SerializeField] float timeToAbsorb = 5;

    public float GetTimeToAbsorb() => timeToAbsorb;
    public float SetTimeToAbsorb(float value) => timeToAbsorb = value;

    [SerializeField] GameObject Remplasar; // replace

    public static Map Instance;

    List<GameObject> map;
    float timer;
    int elegido; // elected
    
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        map = GameObject.FindGameObjectsWithTag("Block").ToList();
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
        DestroyRandomTile();
    }

    /// <summary>
    ///     Chooses a random tile from the map to destroy.
    /// </summary>
    void DestroyRandomTile()
    {
        elegido = Random.Range(0, map.Count);
        Instantiate(Remplasar, map[elegido].transform.position, transform.rotation);
        Destroy(map[elegido]);
        map.RemoveAt(elegido);
    }
}

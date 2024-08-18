using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Map : MonoBehaviour
{
    public GameObject[] map;
    public int MapCount;
    public Transform BlackHole;
    public float Timer;
    public float TimeToAbsorb;
    public int Elegido;
    public GameObject Remplasar;
    public static Map Instance;
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        map = GameObject.FindGameObjectsWithTag("Block");
        BlackHole = GameObject.FindGameObjectWithTag("Destroyer").transform;
    }


    private void FixedUpdate()
    {
        Timer += Time.deltaTime;
        MapCount = map.Length;
    }

    private void Update()
    {
        if (Timer > TimeToAbsorb)
        {
            Timer = 0;
            Elegido = Random.Range(0, MapCount);
            Instantiate(Remplasar, map[Elegido].transform.position, transform.rotation);
            Destroy(map[Elegido]);
        }
    }
}

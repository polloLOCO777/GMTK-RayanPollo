using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleColl : MonoBehaviour
{
    public GameObject BlockGone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            Instantiate(BlockGone, transform.position, transform.rotation);
            transform.SetParent(Map.Instance.InactiveTilesParent);
            gameObject.SetActive(false);
        }
    }
}

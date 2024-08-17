using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class Deleter : MonoBehaviour
{
    public Tilemap destructibleTilemap;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destroyer"))
        {
            Vector3 hitPos = Vector3.zero;
            foreach (ContactPoint2D hit in collision.contacts)
            {
                hitPos.x = hit.point.x - 0.01f * hit.normal.x;
                hitPos.y = hit.point.y - 0.01f * hit.normal.y;
                destructibleTilemap.SetTile(destructibleTilemap.WorldToCell(hitPos), null);
            }
        }
       
    }



}

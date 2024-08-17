using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackround : MonoBehaviour
{
    public float speed;

    public Renderer rendererBg;

    // Update is called once per frame
    void Update()
    {
        rendererBg.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}

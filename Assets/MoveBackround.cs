using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveBackround : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] float speed;

    [Header("Components")]
    [FormerlySerializedAs("rendererBg")]
    [SerializeField] Renderer rendererBackground;

    // Update is called once per frame
    void Update()
        => rendererBackground.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0 * Time.deltaTime);
}

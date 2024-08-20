using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSystem : Singleton<LoadSystem>
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.Log("Loaded systems");
            SceneManager.LoadScene("Systems", LoadSceneMode.Additive);
        }
    }
}

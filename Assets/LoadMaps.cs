using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMaps : MonoBehaviour
{
    public void LoadMap(string map)
    {
        Debug.Log("Loaded Scene");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(map);
    }
}

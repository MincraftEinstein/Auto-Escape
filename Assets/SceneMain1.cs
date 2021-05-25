using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMain1 : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject PlayMenu;
    public void ToMapSelection()
    {
        SceneManager.LoadScene(1);
    }

    public void ModeSelect()
    {
       PlayMenu.SetActive(true);
       MainMenu.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playMenu;
    public GameObject commingSoonText;

    public void LoadScene(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
        SceneManager.UnloadSceneAsync(currentScene);
    }

    private void SwitchMenu(GameObject from, GameObject to)
    {
        from.SetActive(false);
        to.SetActive(true);
    }

    public void PlayButton()
    {
        SwitchMenu(mainMenu, playMenu);
    }

    public void BackButton()
    {
        SwitchMenu(playMenu, mainMenu);
    }

    public void QuitButton()
    {
        Debug.Log("Quiting game");
        Application.Quit();
    }

    public void OptionsMenu()
    {
        StartCoroutine(OptionsMenu2());
    }

    private IEnumerator OptionsMenu2()
    {
        commingSoonText.SetActive(true);
        yield return new WaitForSeconds(1.5F);
        commingSoonText.SetActive(false);
    }
}

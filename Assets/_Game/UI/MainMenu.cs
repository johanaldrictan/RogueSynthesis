using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Andrew Park

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Test Level");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

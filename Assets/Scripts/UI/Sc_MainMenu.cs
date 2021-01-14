using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_MainMenu : MonoBehaviour
{
    public void LaunchLevel()
    {
        SceneManager.LoadScene("Proto");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

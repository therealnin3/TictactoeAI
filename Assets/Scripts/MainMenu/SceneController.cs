using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public void SinglePlayer()
    {
        SceneManager.LoadScene("Game");
        Game.isSinglePlayer = true;
    }
    public void MultiPlayer()
    {
        SceneManager.LoadScene("Game");
        Game.isSinglePlayer = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

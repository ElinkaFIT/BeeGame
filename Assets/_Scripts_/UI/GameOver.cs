//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public static GameOver instance;

    public void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    public void OpenGameOverMenu()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
    }

    public void RestartLevel2()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level_2");
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Pause()
    {
        Time.timeScale = 0;
    }

    public void Continue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }


    
}

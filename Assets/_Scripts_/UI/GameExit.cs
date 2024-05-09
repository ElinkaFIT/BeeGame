//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages game exit and menu actions.
/// </summary>
public class GameExit : MonoBehaviour
{   // Singleton instance of GameExit.
    public static GameExit instance;

    /// <summary>
    /// Initializes the singleton instance and deactivates the game object.
    /// </summary>
    public void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Opens the exit menu and pauses the game.
    /// </summary>
    public void OpenGameExitMenu()
    {
        gameObject.SetActive(true);
        Pause();
    }

    /// <summary>
    /// Restarts the game by reloading the current scene.
    /// </summary>
    public void RestartGame()
    {
        // Resume normal time flow.
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    //public void SaveGame()
    //{
    //    Time.timeScale = 1;
    //    SaveManager.instance.SaveGame();
    //    SceneManager.LoadScene("MainMenu");
    //}


    /// <summary>
    /// Exits the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Pauses the game by setting the time scale to 0.
    /// </summary>
    void Pause()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Continues the game by hiding the exit menu and resuming time.
    /// </summary>
    public void Continue()
    {
        gameObject.SetActive(false);
        // Resume normal time flow.
        Time.timeScale = 1;
    }

    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    public void MainMenu()
    {
        // Resume normal time flow.
        Time.timeScale = 1;
        // Load the "MainMenu" scene.
        SceneManager.LoadScene("MainMenu");
    }
}

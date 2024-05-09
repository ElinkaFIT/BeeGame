//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game over actions and UI.
/// </summary>
public class GameOver : MonoBehaviour
{
    // Singleton instance of GameOver.
    public static GameOver instance; 

    /// <summary>
    /// Initializes the singleton instance and deactivates the game object.
    /// </summary>
    public void Awake()
    {
        instance = this; // Set up the singleton instance.
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Opens the game over menu and pauses the game.
    /// </summary>
    public void OpenGameOverMenu()
    {
        Time.timeScale = 0; // Pause the game.
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Restarts the game by reloading the current scene.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1; // Resume normal time flow.
        SceneManager.LoadScene("Game");
        Time.timeScale = 1; // Resume normal time flow.
    }

    /// <summary>
    /// Restarts the game at level 2 by loading the "Level_2" scene.
    /// </summary>
    public void RestartLevel2()
    {
        Time.timeScale = 1; // Resume normal time flow.
        SceneManager.LoadScene("Level_2");
        Time.timeScale = 1; // Resume normal time flow.
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Stop time flow.
    /// </summary>
    void Pause()
    {
        Time.timeScale = 0; // Pause the game.
    }

    /// <summary>
    /// Continues the game by hiding the game over menu and resuming time.
    /// </summary>
    public void Continue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1; // Resume normal time flow.
    }

    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1; // Resume normal time flow.
        SceneManager.LoadScene("MainMenu");
    }
}

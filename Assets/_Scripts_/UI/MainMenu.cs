//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the main menu functionality.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Starts a new game by loading the main game scene.
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1;  
        SceneManager.LoadScene("Game");  
    }

    /// <summary>
    /// Starts a game from a saved state.
    /// </summary>
    public void StartSavedGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        SaveManager.instance.LoadGame();  
    }

    /// <summary>
    /// Starts the campaign by loading the first level of the campaign.
    /// </summary>
    public void StartCampaign()
    {
        Time.timeScale = 1; 
        SceneManager.LoadScene("OpeningLevel_1");
    }

    /// <summary>
    /// Navigates to the settings menu.
    /// </summary>
    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }
}

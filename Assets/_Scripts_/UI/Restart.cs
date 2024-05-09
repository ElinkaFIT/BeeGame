//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the game restarting mechanism by loading a specific scene.
/// </summary>
public class Restart : MonoBehaviour
{
    /// <summary>
    /// Restarts the game by reloading the 'Game' scene.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}

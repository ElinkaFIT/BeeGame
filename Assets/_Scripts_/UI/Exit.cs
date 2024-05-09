//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************;
using UnityEngine;

/// <summary>
/// Provides functionality to exit the game.
/// </summary>
public class Exit : MonoBehaviour
{
    /// <summary>
    /// Exits the game when called.
    /// </summary>
    public void ExitGame()
    {
        // Exits the game.
        Application.Quit();
    }
}

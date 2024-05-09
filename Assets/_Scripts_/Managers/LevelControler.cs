//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the transition to the next level in the game.
/// </summary>
public class LevelControler : MonoBehaviour
{
    // Index of the next scene.
    public int nextSceneBuildIndex;

    /// <summary>
    /// Loads the next level based on the specified scene build index.
    /// </summary>
    public void NextLevel()
    {
        // Load the scene with the given index.
        SceneManager.LoadScene(nextSceneBuildIndex);
    }
}

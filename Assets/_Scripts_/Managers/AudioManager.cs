//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Manages the audio across different scenes and ensures that only one AudioManager exists.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Singleton instance of AudioManager.
    public static AudioManager instance;

    /// <summary>
    /// Ensures that only one instance of AudioManager exists throughout the game.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Keeps the AudioManager persistent across scenes.
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Destroys this instance if another AudioManager already exists.
            Destroy(this.gameObject);
        }
    }
}

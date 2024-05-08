//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 
/// </summary>
public class Restart : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}

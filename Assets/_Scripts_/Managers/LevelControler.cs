//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControler : MonoBehaviour
{
    public int nextSceneBuildIndex;
    
    public void NextLevel()
    {
        SceneManager.LoadScene(nextSceneBuildIndex);
    }
}

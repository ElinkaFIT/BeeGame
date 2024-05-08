//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void StartSavedGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
        SaveManager.instance.LoadGame();
    }

    public void StartCampaign()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("OpeningLevel_1");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

}

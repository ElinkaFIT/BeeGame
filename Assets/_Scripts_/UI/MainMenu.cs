using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartSavedGame()
    {
        SceneManager.LoadScene("Game");
        SaveManager.instance.LoadGame();
    }

    public void StartCampaign()
    {
        SceneManager.LoadScene("OpeningLevel_1");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

}

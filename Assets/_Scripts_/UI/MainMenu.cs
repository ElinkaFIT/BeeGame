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

    public void StartCampaign()
    {
        SceneManager.LoadScene("TutorialPreset");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
    }

}

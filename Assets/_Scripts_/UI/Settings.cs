using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Screen = UnityEngine.Device.Screen;

public class Settings : MonoBehaviour
{
    Resolution[] resolutions;

    public TMP_Dropdown dropdown;

    private void Start()
    {
        resolutions = Screen.resolutions;

        dropdown.ClearOptions();

        List<string> options = new List<string>();

        int curResolution = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
               curResolution = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = curResolution;
        dropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}

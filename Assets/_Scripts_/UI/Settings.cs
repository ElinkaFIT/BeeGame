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
    private Resolution[] pcResolutions;
    public List<Resolution> myResolutions;
    private bool isFullscreenOn;

    public TMP_Dropdown dropdown;

    private void Start()
    {
        isFullscreenOn = true;
        myResolutions = new List<Resolution>();

        pcResolutions = Screen.resolutions;

        dropdown.ClearOptions();

        List<string> options = new List<string>();
        HashSet<string> uniqueResolutions = new HashSet<string>();

        int curResolution = 0;
        for (int i = 0; i < pcResolutions.Length; i++) {

            string option = pcResolutions[i].width + "x" + pcResolutions[i].height;


            if (uniqueResolutions.Add(option) && pcResolutions[i].width > 1000)
            {
                myResolutions.Add(pcResolutions[i]); 
                options.Add(option);

                if (pcResolutions[i].width == Screen.currentResolution.width && pcResolutions[i].height == Screen.currentResolution.height)
                {
                    curResolution = options.Count - 1;  
                }
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = curResolution;
        dropdown.RefreshShownValue();

    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = myResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, isFullscreenOn);
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        isFullscreenOn = isFullScreen;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}

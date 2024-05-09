//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Screen = UnityEngine.Device.Screen;

/// <summary>
/// Manages the settings for the game, including resolution and fullscreen preferences.
/// </summary>
public class Settings : MonoBehaviour
{
    private Resolution[] pcResolutions;              // All possible resolutions for this PC.
    public List<Resolution> myResolutions;           // Custom list of valid resolutions.
    private bool isFullscreenOn;                     // Current fullscreen status.

    public TMP_Dropdown dropdown;                    // Dropdown menu for selecting screen resolution.

    /// <summary>
    /// Initializes settings by loading available screen resolutions and setting up the dropdown menu.
    /// </summary>
    private void Start()
    {
        isFullscreenOn = true; 
        myResolutions = new List<Resolution>();

        pcResolutions = Screen.resolutions; // Get all available resolutions from the system.

        dropdown.ClearOptions();

        List<string> options = new List<string>();
        HashSet<string> uniqueResolutions = new HashSet<string>();

        int curResolution = 0;
        for (int i = 0; i < pcResolutions.Length; i++)
        {
            string option = pcResolutions[i].width + "x" + pcResolutions[i].height;

            // Check for unique and sufficiently large resolutions.
            if (uniqueResolutions.Add(option) && pcResolutions[i].width > 1000)
            {
                myResolutions.Add(pcResolutions[i]);
                options.Add(option); // Add to dropdown options.

                if (pcResolutions[i].width == Screen.currentResolution.width &&
                    pcResolutions[i].height == Screen.currentResolution.height)
                {
                    curResolution = options.Count - 1;
                }
            }
        }

        dropdown.AddOptions(options); 
        dropdown.value = curResolution;
        dropdown.RefreshShownValue();   // Update the UI.
    }

    /// <summary>
    /// Sets the game resolution based on the selected index from the dropdown.
    /// </summary>
    /// <param name="resolutionIndex">Index of the chosen resolution.</param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = myResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, isFullscreenOn);
    }

    /// <summary>
    /// Toggles fullscreen mode.
    /// </summary>
    /// <param name="isFullScreen">Whether the screen should be set to fullscreen.</param>
    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        isFullscreenOn = isFullScreen;
    }

    /// <summary>
    /// Returns to the main menu scene.
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the background music volume using a UI slider.
/// </summary>
public class MusicSlider : MonoBehaviour
{
    // The slider component used to adjust the music volume.
    public Slider slider;

    // The AudioSource component which plays the music.
    private AudioSource audioSource; 

    /// <summary>
    /// Initializes the slider's value based on the current music volume and sets up the listener for value changes.
    /// </summary>
    void Start()
    {
        // Find the AudioSource in the scene
        audioSource = FindObjectOfType<AudioSource>();

        if (audioSource != null)
        {
            slider.value = audioSource.volume;
        }

        // Add a listener to handle volume changes when the slider's value changes
        slider.onValueChanged.AddListener((value) =>
        {
            if (audioSource != null)
            {
                //audioSource.mute = true;
                audioSource.volume = value;
            }
        });
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

    }
}

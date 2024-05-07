//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    public Slider slider;
    private AudioSource audioSource;

    private 
    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();

        if (audioSource != null)
        {
            slider.value = audioSource.volume;
        }


        slider.onValueChanged.AddListener((value) =>
        {
            if (audioSource != null)
            {
                //audioSource.mute = true;
                audioSource.volume = value;
            }

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

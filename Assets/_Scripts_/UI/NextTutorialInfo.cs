//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Controls the display of tutorial images, allowing the user to cycle through them.
/// </summary>
public class NextTutorialInfo : MonoBehaviour
{
    // The GameObject where the tutorial image is displayed.
    public GameObject displayImage;        

    // The GameObject where the tutorial image is displayed.
    public GameObject[] imageFiles;       

    // The index of the current image being displayed.
    private int currentIndex;             

    // Reference to the current image GameObject being displayed.
    private GameObject currentImage;        

    /// <summary>
    /// Initializes the tutorial by displaying the first image.
    /// </summary>
    private void Start()
    {
        currentIndex = -1; 
        NextImage();    
    }

    /// <summary>
    /// Advances to the next image in the tutorial sequence.
    /// </summary>
    public void NextImage()
    {
        // Check if there is a next image to display
        if (currentIndex + 1 < imageFiles.Length)
        {
            if (currentImage != null)
                Destroy(currentImage);

            currentIndex++;
            currentImage = Instantiate(imageFiles[currentIndex], displayImage.transform);
        }
    }

    /// <summary>
    /// Moves back to the recent image in the tutorial sequence.
    /// </summary>
    public void RecentImage()
    {
        // Check if there is a previous image to display
        if (currentIndex - 1 >= 0)
        {
            if (currentImage != null)
                Destroy(currentImage);

            currentIndex--;
            currentImage = Instantiate(imageFiles[currentIndex], displayImage.transform);
        }
    }
}

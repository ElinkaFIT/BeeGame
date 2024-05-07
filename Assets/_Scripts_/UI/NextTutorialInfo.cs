//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

public class NextTutorialInfo : MonoBehaviour
{
    public GameObject displayImage;
    public GameObject[] imageFiles;

    private int currentIndex;
    private GameObject currentImage;

    private void Start()
    {
        currentIndex = -1;
        NextImage();
    }

    public void NextImage()
    {

        if (currentIndex + 1 < imageFiles.Length)
        {
            Destroy(currentImage);
            currentIndex++;
            currentImage = Instantiate(imageFiles[currentIndex], displayImage.transform);
        }
        
    }

    public void RecentImage()
    {

        if (currentIndex - 1 >= 0 && currentIndex - 1 < imageFiles.Length)
        {
            Destroy(currentImage);
            currentIndex--;
            currentImage = Instantiate(imageFiles[currentIndex], displayImage.transform);
        }

    }
}

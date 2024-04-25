using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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

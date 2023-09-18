using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProgressBar : MonoBehaviour
{
    public GameObject progressContainer;
    public RectTransform progressFill;
    private float maxSize;

    void Awake()
    {
        maxSize = progressFill.sizeDelta.x;
        progressContainer.SetActive(false);
    }

    public void UpdateProgressBar(int curProgress, int maxProgress)
    {
        progressContainer.SetActive(true);
        float progressPercentage = (float)curProgress / (float)maxProgress;
        progressFill.sizeDelta = new Vector2(maxSize * progressPercentage, progressFill.sizeDelta.y);

    }

    public void CloseProgressBar() 
    { 
        progressContainer.SetActive(false); 
    }
}

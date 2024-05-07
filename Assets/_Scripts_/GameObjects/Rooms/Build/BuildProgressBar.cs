//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
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

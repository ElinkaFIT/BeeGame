//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Manages the display of a progress bar for build operations in the game.
/// </summary>
public class BuildProgressBar : MonoBehaviour
{
    public GameObject progressContainer;    // The container that holds the progress bar, used to show/hide the bar
    public RectTransform progressFill;      // The UI element that represents the filled part of the progress bar
    private float maxSize;                  // The maximum width of the progress bar, representing 100% completion

    /// <summary>
    /// Initializes the progress bar, setting the maximum size and hiding the bar initially.
    /// </summary>
    void Awake()
    {
        maxSize = progressFill.sizeDelta.x;  
        progressContainer.SetActive(false); 
    }

    /// <summary>
    /// Updates the progress bar's width according to the current progress relative to the maximum progress.
    /// </summary>
    /// <param name="curProgress">The current progress value.</param>
    /// <param name="maxProgress">The maximum possible progress value.</param>
    public void UpdateProgressBar(int curProgress, int maxProgress)
    {
        progressContainer.SetActive(true);
        float progressPercentage = (float)curProgress / (float)maxProgress;
        progressFill.sizeDelta = new Vector2(maxSize * progressPercentage, progressFill.sizeDelta.y);
    }

    /// <summary>
    /// Hides the progress bar.
    /// </summary>
    public void CloseProgressBar()
    {
        progressContainer.SetActive(false);
    }
}

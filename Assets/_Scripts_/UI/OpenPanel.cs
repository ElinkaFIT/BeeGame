//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Controls the visibility of a UI panel, allowing it to be opened or closed.
/// </summary>
public class OpenPanel : MonoBehaviour
{
    // The panel GameObject that this script will show or hide.
    public GameObject panel;       

    /// <summary>
    /// Sets the initial state of the panel to be hidden.
    /// </summary>
    private void Awake()
    {
        panel.SetActive(false);
    }

    /// <summary>
    /// Toggles the visibility of the panel.
    /// </summary>
    public void OpenClosePanel()
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }
}

//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class OpenPanel : MonoBehaviour
{
    public GameObject panel;    // object of some panel

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        panel.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OpenClosePanel()
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }
}

//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    public GameObject panel;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void OpenClosePanel()
    {
        bool isActive = panel.activeSelf;
        panel.SetActive(!isActive);
    }
}

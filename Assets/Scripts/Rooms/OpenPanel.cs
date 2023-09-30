using System.Collections;
using System.Collections.Generic;
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

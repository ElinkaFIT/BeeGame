using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI nectarText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI waxText;
    public TextMeshProUGUI propolisText;

    public static GameUI instance;
    void Awake()
    {
        instance = this;
    }

    public void UpdateNectarText(int value)
    {
        nectarText.text = value.ToString();
    }

    public void UpdateWaterText(int value)
    {
        waterText.text = value.ToString();
    }

    public void UpdateWaxText(int value)
    {
        waxText.text = value.ToString();
    }

    public void UpdatePropolisText(int value)
    {
        propolisText.text = value.ToString();
    }
}

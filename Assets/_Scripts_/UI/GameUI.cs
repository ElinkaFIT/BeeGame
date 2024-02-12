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
    public TextMeshProUGUI beesText;
    public TextMeshProUGUI enemyText;

    public TextMeshProUGUI nectarCapacity;
    public TextMeshProUGUI waterCapacity;
    public TextMeshProUGUI waxCapacity;
    public TextMeshProUGUI propolisCapacity;

    public static GameUI instance;
    void Awake()
    {
        instance = this;
    }

    public void UpdateNectarText(int value)
    {
        nectarText.text = value.ToString();
    }

    public void UpdateNectarCapacity(int value)
    {
        nectarCapacity.text = "/ " + value.ToString();
    }

    public void UpdateWaterText(int value)
    {
        waterText.text = value.ToString();
    }

    public void UpdateWaterCapacity(int value)
    {
        waterCapacity.text = "/ " + value.ToString();
    }

    public void UpdateWaxText(int value)
    {
        waxText.text = value.ToString();
    }

    public void UpdateWaxCapacity(int value)
    {
        waxCapacity.text = " /" + value.ToString();
    }

    public void UpdatePropolisText(int value)
    {
        propolisText.text = value.ToString();
    }

    public void UpdatePropolisCapacity(int value)
    {
        propolisCapacity.text = "/ " + value.ToString();
    }

    public void UpdateBeesText(int value)
    {
        beesText.text = value.ToString();
    }
    public void UpdateEnemyText(int value)
    {
        enemyText.text = value.ToString();
    }
}

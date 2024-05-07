//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI nectarText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI waxText;
    public TextMeshProUGUI pollenText;
    public TextMeshProUGUI honeyText;

    public TextMeshProUGUI nectarCapacity;
    public TextMeshProUGUI waterCapacity;
    public TextMeshProUGUI waxCapacity;
    public TextMeshProUGUI pollenCapacity;

    public TextMeshProUGUI beesText;
    public TextMeshProUGUI enemyText;

    public static GameUI instance;
    void Awake()
    {
        instance = this;
    }

    // Nectar
    public void UpdateNectarText(int value)
    {
        nectarText.text = "Nectar: " + value.ToString();
    }

    public void UpdateNectarCapacity(int value)
    {
        nectarCapacity.text = "/ " + value.ToString();
    }

    // Water
    public void UpdateWaterText(int value)
    {
        waterText.text = "Water: " + value.ToString();
    }

    public void UpdateWaterCapacity(int value)
    {
        waterCapacity.text = "/ " + value.ToString();
    }

    // Wax
    public void UpdateWaxText(int value)
    {
        waxText.text = "Wax: " + value.ToString();
    }

    public void UpdateWaxCapacity(int value)
    {
        waxCapacity.text = " /" + value.ToString();
    }

    // Pollen
    public void UpdatePollenText(int value)
    {
        pollenText.text = "Pollen: " + value.ToString();
    }

    public void UpdatePollenCapacity(int value)
    {
        pollenCapacity.text = "/ " + value.ToString();
    }

    // Honey
    public void UpdateHoneyText(int value)
    {
        honeyText.text = "Honey: " + value.ToString();
    }

    // Units
    public void UpdateBeesText(int value)
    {
        beesText.text = "Bees: " + value.ToString();
    }
    public void UpdateEnemyText(int value)
    {
        enemyText.text = value.ToString();
    }
}

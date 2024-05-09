//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the display of various game resources and unit counts in the UI.
/// </summary>
public class GameUI : MonoBehaviour
{
    public TextMeshProUGUI nectarText;      // UI elements to display the current amount of nectar
    public TextMeshProUGUI waterText;       // UI elements to display the current amount of water
    public TextMeshProUGUI waxText;         // UI elements to display the current amount of wax
    public TextMeshProUGUI pollenText;      // UI elements to display the current amount of pollen
    public TextMeshProUGUI honeyText;       // UI elements to display the current amount of honey

    public TextMeshProUGUI nectarCapacity;  // UI elements to display the current caaximum capacity for nectar
    public TextMeshProUGUI waterCapacity;   // UI elements to display the current caaximum capacity for water
    public TextMeshProUGUI waxCapacity;     // UI elements to display the current caaximum caaximum pacity for wax
    public TextMeshProUGUI pollenCapacity;  // UI elements to display the current caaximum capacity for pollen

    public TextMeshProUGUI beesText;        // UI elements to display the current number of bees
    public TextMeshProUGUI enemyText;       // UI elements to display the current number of enemies.

    public static GameUI instance;          // Singleton instance of GameUI.

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Updates the text display of the current nectar amount.
    /// </summary>
    /// <param name="value">The current nectar amount.</param>
    public void UpdateNectarText(int value)
    {
        nectarText.text = "Nectar: " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the maximum nectar capacity.
    /// </summary>
    /// <param name="value">The maximum nectar capacity.</param>
    public void UpdateNectarCapacity(int value)
    {
        nectarCapacity.text = "/ " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the current water amount.
    /// </summary>
    /// <param name="value">The current water amount.</param>
    public void UpdateWaterText(int value)
    {
        waterText.text = "Water: " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the maximum water capacity.
    /// </summary>
    /// <param name="value">The maximum water capacity.</param>
    public void UpdateWaterCapacity(int value)
    {
        waterCapacity.text = "/ " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the current wax amount.
    /// </summary>
    /// <param name="value">The current wax amount.</param>
    public void UpdateWaxText(int value)
    {
        waxText.text = "Wax: " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the maximum wax capacity.
    /// </summary>
    /// <param name="value">The maximum wax capacity.</param>
    public void UpdateWaxCapacity(int value)
    {
        waxCapacity.text = " /" + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the current pollen amount.
    /// </summary>
    /// <param name="value">The current pollen amount.</param>
    public void UpdatePollenText(int value)
    {
        pollenText.text = "Pollen: " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the maximum pollen capacity.
    /// </summary>
    /// <param name="value">The maximum pollen capacity.</param>
    public void UpdatePollenCapacity(int value)
    {
        pollenCapacity.text = "/ " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the current honey amount.
    /// </summary>
    /// <param name="value">The current honey amount.</param>
    public void UpdateHoneyText(int value)
    {
        honeyText.text = "Honey: " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the current number of bees.
    /// </summary>
    /// <param name="value">The current number of bees.</param>
    public void UpdateBeesText(int value)
    {
        beesText.text = "Bees: " + value.ToString();
    }

    /// <summary>
    /// Updates the text display of the current number of enemies.
    /// </summary>
    /// <param name="value">The current number of enemies.</param>
    public void UpdateEnemyText(int value)
    {
        enemyText.text = value.ToString();
    }
}

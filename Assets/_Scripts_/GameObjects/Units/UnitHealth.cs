//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Manages the health bar UI for a unit.
/// </summary>
public class UnitHealth : MonoBehaviour
{
    public GameObject healthContainer;           // Container holding the health bar UI.
    public RectTransform healthFill;             // UI element showing the current health.
    private float maxSize                        // Maximum width of the health bar.

    /// <summary>
    /// Initialize health bar and set initial visibility.
    /// </summary>
    void Awake()
    {
        maxSize = healthFill.sizeDelta.x; // Set the max size from the initial health bar width.
        healthContainer.SetActive(false); // Hide health bar initially.
    }

    /// <summary>
    /// Updates the health bar based on current and max health.
    /// </summary>
    /// <param name="curHp">Current health.</param>
    /// <param name="maxHp">Maximum health.</param>
    public void UpdateHealthBar(int curHp, int maxHp)
    {
        healthContainer.SetActive(true);

        float healthPercentage = (float)curHp / (float)maxHp;
        healthFill.sizeDelta = new Vector2(maxSize * healthPercentage, healthFill.sizeDelta.y);
    }
}

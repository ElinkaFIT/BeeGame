//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the player's properties, including units and game over conditions.
/// </summary>
public class Player : MonoBehaviour
{
    public bool isMe;                                   // Flag indicating if this is the main player.
    public static Player me;                            // Static reference to the main player.
    public GameOver gameOver;                           // Reference to the GameOver script to trigger the end game scenario.
    public TextMeshPro beeCount;                        // Text element to display the number of bees.

    [Header("Units")]
    public List<Unit> units = new List<Unit>();         // List of all units that belong to the player.

    /// <summary>
    /// Placeholder for future initialization code.
    /// </summary>
    void Start()
    {
        // GameUI.instance.UpdateUnitCountText(units.Count);
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        if (isMe)
        {
            me = this;
        }
    }

    /// <summary>
    /// Updates the bee count and checks for the end game condition.
    /// </summary>
    private void Update()
    {
        // Update the displayed bee count.
        GameUI.instance.UpdateBeesText(me.units.Count);

        // Check if there are no units left and trigger the game over
        if (me.units.Count == 0)
        {
            gameOver.OpenGameOverMenu();
        }
    }

    /// <summary>
    /// Checks if the specified unit belongs to this player.
    /// </summary>
    /// <param name="unit">The unit to check.</param>
    /// <returns>True if the unit belongs to this player, otherwise false.</returns>
    public bool IsMyUnit(Unit unit)
    {
        return units.Contains(unit);
    }

    /// <summary>
    /// Method that will create a new unit.
    /// </summary>
    public void CreateNewUnit()
    {
    }
}

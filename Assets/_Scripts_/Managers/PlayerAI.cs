//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the enemy player, including spawning and managing units.
/// </summary>
public class PlayerAI : MonoBehaviour
{
    // Singleton instance of the enemy AI.
    public static PlayerAI enemy;

    [Header("Spawn Settings")]
    public float minSpawnRate;                      // Minimum rate at which units are spawned.
    public float maxSpawnRate;                      // Maximum rate at which units are spawned.

    [Header("Units")]
    public List<UnitAI> units = new List<UnitAI>(); // List of all enemy units.
    public GameObject unitPrefab;                   // Prefab for a standard enemy unit.
    public GameObject hiveUnitPrefab;               // Prefab for a hive-specific enemy unit.

    // Reference to the CommandManager.
    public CommandManager commandManager; 

    /// <summary>
    /// Initializes the enemy player and potentially schedules initial commands.
    /// </summary>
    void Start()
    {
        enemy = this;
        // Example of adding initial commands if needed.
        // commandManager.AddSpawnEnemy();
        // commandManager.AddSpawnHiveEnemy();
    }

    /// <summary>
    /// Updates the UI with the current number of enemy units.
    /// </summary>
    void Update()
    {
        GameUI.instance.UpdateEnemyText(units.Count);
    }

    /// <summary>
    /// Checks if the given unit belongs to this enemy player.
    /// </summary>
    /// <param name="unit">The unit to check.</param>
    /// <returns>True if the unit belongs to this enemy player, otherwise false.</returns>
    public bool IsMyUnit(UnitAI unit)
    {
        return units.Contains(unit);
    }

    /// <summary>
    /// Spawns a standard enemy unit at a fixed location.
    /// </summary>
    public void SpawnUnit()
    {
        Vector3 spawnPos = new(5, -3, 0); // Fixed spawn position.
        GameObject unitObj = Instantiate(unitPrefab, spawnPos, Quaternion.identity);

        UnitAI unit = unitObj.GetComponent<UnitAI>();
        units.Add(unit);
    }

    /// <summary>
    /// Spawns a hive-specific enemy unit at a fixed location.
    /// </summary>
    public void SpawnHiveUnit()
    {
        Vector3 spawnPos = new(-20, -20, 0); // Fixed spawn position for hive units.
        GameObject unitObj = Instantiate(hiveUnitPrefab, spawnPos, Quaternion.identity);

        UnitAI unit = unitObj.GetComponent<UnitAI>();
        units.Add(unit);
    }
}

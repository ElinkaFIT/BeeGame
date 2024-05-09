//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// CommandManager is responsible for managing and executing commands related to enemy spawning.
/// </summary>
public class CommandManager : MonoBehaviour
{
    // Reference to the PlayerAI that receives the spawn commands
    public PlayerAI spawnReciever;

    // Component that saves commands for later execution or logging
    public CommandSaver commandSaver;

    // Singleton instance of the CommandManager
    public static CommandManager instance;

    /// <summary>
    /// Sets up the singleton instance of the CommandManager.
    /// </summary>
    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Executes the command to spawn a regular enemy unit.
    /// </summary>
    public void RunSpawnEnemy()
    {
        if (spawnReciever == null)
        {
            return;
        }

        // Log the action of spawning a regular enemy
        Debug.Log("enemy unit");

        // Add a new command to the command saver with a delay
        commandSaver.AddNewCommand(Time.time + 200, "Enemy");
    }

    /// <summary>
    /// Executes the command to spawn a hive-specific enemy unit.
    /// </summary>
    public void RunSpawnHiveEnemy()
    {
        if (spawnReciever == null)
        {
            return;
        }

        // Log the action of spawning a hive-specific enemy
        Debug.Log("enemy unit hive");

        // Add a new command to the command saver with a delay
        commandSaver.AddNewCommand(Time.time + 20, "HiveEnemy");
    }
}

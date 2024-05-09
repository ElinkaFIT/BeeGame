//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Command to spawn enemy type A using the command pattern.
/// </summary>
public class SpawnEnemyACommand : ICommand
{
    // Reference to the PlayerAI controller
    private PlayerAI spawnReceiver; 

    /// <summary>
    /// Initializes a new instance of the <see cref="SpawnEnemyACommand"/> class.
    /// </summary>
    /// <param name="receiver">The PlayerAi reference that will process the spawn command.</param>
    public SpawnEnemyACommand(PlayerAI receiver)
    {
        this.spawnReceiver = receiver; // Store the reference to the receiver
    }

    /// <summary>
    /// Executes the command to spawn an enemy unit of type A.
    /// </summary>
    public void Execute()
    {
        // Delegate the spawning of the unit to the receiver
        spawnReceiver.SpawnUnit();

        // Log this spawning action with a timestamp and a message
        Log.instance.AddNewLogText(Time.time, "New enemy", Color.black);
    }
}
